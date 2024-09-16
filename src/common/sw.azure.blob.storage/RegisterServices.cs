using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sw.azure.blob.storage
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterAzureBlobStorageServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IBlobService, BlobService>();

            return services;
        }
        
    }//Class : 
    
}//Namespace : sw.azure.blob.storage
