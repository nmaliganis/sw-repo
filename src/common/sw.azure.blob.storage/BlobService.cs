using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace sw.azure.blob.storage
{
    public class BlobService : IBlobService
    {
        /// <summary>
        /// Class BlobServiceClient
        /// </summary>
        private BlobServiceClient _blobServiceClient;

        /// <summary>
        /// 
        /// </summary>
        
        protected string _connectionString;
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString
        {
            set
            {
                if (value != null) _connectionString = value;
                _blobServiceClient ??= new BlobServiceClient(_connectionString);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<Uri> UploadFileBlobAsync(string blobContainerName, Stream content, string contentType, string fileName)
        {
            if (blobContainerName == null) throw new ArgumentNullException(nameof(blobContainerName));

            var containerClient = GetContainerClient(blobContainerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType });

            return blobClient.Uri;
        }
        
        /// <summary>
        /// Method : DownloadFile
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<BlobDownloadInfo> DownloadFile(string blobContainerName, string fileName)
        {
            if (blobContainerName == null) throw new ArgumentNullException(nameof(blobContainerName));

            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            var containerClient = GetContainerClient(blobContainerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            var exist = await blobClient.ExistsAsync();
            if (!exist) return null;

            var blobDownloadInfo = await blobClient.DownloadAsync();

            return blobDownloadInfo;

        }

        public async Task<Response> DeleteFile(string blobContainerName, string fileName)
        {
            if (blobContainerName == null) throw new ArgumentNullException(nameof(blobContainerName));

            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            var containerClient = GetContainerClient(blobContainerName);

            var blobClient = containerClient.GetBlobClient(fileName);

            var exist = await blobClient.ExistsAsync();

            if (!exist) return null;

            var response = await blobClient.DeleteAsync();

            return response;
        }

        /// <summary>
        /// Method : GetContainerClient
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <returns></returns>
        private BlobContainerClient GetContainerClient(string blobContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(blobContainerName);

            containerClient.CreateIfNotExists(PublicAccessType.Blob);

            return containerClient;
        }

        public async Task<string> ReadFileContent(string blobContainerName, string fileName)
        {
            if (blobContainerName == null) throw new ArgumentNullException(nameof(blobContainerName));

            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            var containerClient = GetContainerClient(blobContainerName);

            var blobClient = containerClient.GetBlobClient(fileName);
            StringBuilder sb = new StringBuilder();
            var response = await blobClient.DownloadAsync();
            using (var streamReader = new StreamReader(response.Value.Content))
            {
                while (!streamReader.EndOfStream)
                {
                    sb.AppendLine(await streamReader.ReadLineAsync());
                }
            }
            return sb.ToString();

        }
        public async Task<List<string>> GetContainerFiles(string blobContainerName)
        {
            if (blobContainerName == null) throw new ArgumentNullException(nameof(blobContainerName));

            var containerClient = GetContainerClient(blobContainerName);

            List<string> files = new List<string>();
            // List all blobs in the container
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                files.Add(blobItem.Name);
            }

            return files;
        }
        public async Task<bool> MoveFile(string blobContainerName, string blobDestinationContainerName, string fileName)
        {
            if (blobContainerName == null) throw new ArgumentNullException(nameof(blobContainerName));
            if (blobDestinationContainerName == null) throw new ArgumentNullException(nameof(blobDestinationContainerName));
            try
            {

                var containerClient = GetContainerClient(blobContainerName);
                var destinationContainerClient = GetContainerClient(blobDestinationContainerName);

                // Create a BlobClient representing the source blob to copy.
                BlobClient sourceBlob = containerClient.GetBlobClient(fileName);

                // Ensure that the source blob exists.
                if (await sourceBlob.ExistsAsync())
                {
                    // Lease the source blob for the copy operation 
                    // to prevent another client from modifying it.
                    BlobLeaseClient lease = sourceBlob.GetBlobLeaseClient();

                    // Specifying -1 for the lease interval creates an infinite lease.
                    await lease.AcquireAsync(TimeSpan.FromSeconds(-1));

                    // Get the source blob's properties and display the lease state.
                    BlobProperties sourceProperties = await sourceBlob.GetPropertiesAsync();
                    Console.WriteLine($"Lease state: {sourceProperties.LeaseState}");

                    // Get a BlobClient representing the destination blob with a unique name.
                    BlobClient destBlob = destinationContainerClient.GetBlobClient(fileName);

                    // Start the copy operation.
                    await destBlob.StartCopyFromUriAsync(sourceBlob.Uri);

                    // Get the destination blob's properties and display the copy status.
                    BlobProperties destProperties = await destBlob.GetPropertiesAsync();

                    Console.WriteLine($"Copy status: {destProperties.CopyStatus}");
                    Console.WriteLine($"Copy progress: {destProperties.CopyProgress}");
                    Console.WriteLine($"Completion time: {destProperties.CopyCompletedOn}");
                    Console.WriteLine($"Total bytes: {destProperties.ContentLength}");

                    // Update the source blob's properties.
                    sourceProperties = await sourceBlob.GetPropertiesAsync();

                    if (sourceProperties.LeaseState == LeaseState.Leased)
                    {
                        // Break the lease on the source blob.
                        await lease.BreakAsync();

                        // Update the source blob's properties to check the lease state.
                        sourceProperties = await sourceBlob.GetPropertiesAsync();
                        Console.WriteLine($"Lease state: {sourceProperties.LeaseState}");
                    }

                }
                await DeleteFile(blobContainerName, fileName);
                return true;
            }
            catch
            {
                //Todo: Create Handling
            }
            return false;
        }
    }
}
