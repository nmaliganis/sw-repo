using Azure.Messaging.ServiceBus;
using sw.auth.common.infrastructure.Commanding.Models;
using sw.infrastructure.Azure;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sw.auth.api.Configurations.Installers;

internal static class AbsInstaller
{
    public static IServiceCollection RegisterAsbAsPublisherServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = ConfigureBus(configuration);

        if (config.TopicDriver != null)
        {
            services.AddMassTransit<IBus>(x =>
            {
                x.AddServiceBusMessageScheduler();
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingAzureServiceBus((_, cfg) =>
                {
                    cfg.Host(config.EndPoint, h =>
                    {
                        h.RetryLimit = 3;
                        h.TransportType = ServiceBusTransportType.AmqpTcp;
                    });
                    cfg.UseServiceBusMessageScheduler();

                    cfg.Send<DriverCreationReceived>(s
                        => s.UseSessionIdFormatter(c => c.Message.CorrelationId.ToString("D")));
                });
            });
        }

        services.AddMassTransitHostedService();
        return services;
    }

    private static AsbConfig ConfigureBus(IConfiguration configuration)
    {
        AsbConfig config = new AsbConfig()
        {
            EndPoint = configuration.GetSection("ASB:EndPoint")!.Value,
            SasLocator = configuration.GetSection("ASB:SasLocator")!.Value,
            TopicDriver = configuration.GetSection("ASB:TopicDriver")!.Value,
        };
        return config;
    }
}