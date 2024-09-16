using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;

namespace sw.azure.blob.storage
{
    public interface IBlobService
    {
        /// <summary>
        /// Method : UploadFileBlobAsync
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<Uri> UploadFileBlobAsync(string blobContainerName, Stream content, string contentType, string fileName);
        /// <summary>
        /// Property : ConnectionString
        /// </summary>
        public string ConnectionString { set; }
        /// <summary>
        /// Method : DownloadFile
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<BlobDownloadInfo> DownloadFile(string blobContainerName, string fileName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobContainerName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<Response> DeleteFile(string blobContainerName, string fileName);

        Task<string> ReadFileContent(string blobContainerName, string fileName);

        Task<List<string>> GetContainerFiles(string blobContainerName);

        Task<bool> MoveFile(string blobContainerName, string blobDestinationContainerName, string fileName);
        
    }//Interface : IBlobService
    
}//Namespace : sw.azure.blob.storage