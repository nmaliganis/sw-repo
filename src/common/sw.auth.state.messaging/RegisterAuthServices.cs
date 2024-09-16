using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sw.auth.state.messaging
{
    public static class RegisterAuthServices
    {
        public static IServiceCollection RegisterAzureBlobStorageServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IAuthMessageServiceBus, AuthAuthMessageServiceBusService>();

            return services;
        }
    }
}