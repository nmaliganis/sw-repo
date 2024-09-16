using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sw.auth.messaging.Providers.RabbitMq
{
    public static class RegisterAuthRabbitMqServices
    {
        public static IServiceCollection RegisterAzureRabbitMqServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IAuthMessageServiceBus, AuthMessageRabbitMqServiceBusService>();

            return services;
        }
    }
}