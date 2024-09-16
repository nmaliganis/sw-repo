using Azure.Messaging.ServiceBus;
using sw.routing.api.Commanding;
using sw.infrastructure.Azure;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sw.routing.api.Configurations.Installers;

internal static class AbsInstaller
{
    public static IServiceCollection RegisterAsbAsConsumerServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = ConfigureBus(configuration);

        services.AddMassTransit(x =>
        {
            x.AddServiceBusMessageScheduler();
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumer<DriverCreationConsumer>();

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

                cfg.SubscriptionEndpoint<DriverCreationReceived>($"{config.Topic}-consumer",
                    e => { e.ConfigureConsumer<DriverCreationConsumer>(_); });

                cfg.ConfigureEndpoints(_);
            });
        });

        services.AddMassTransitHostedService(true);
        services.AddHealthChecks().AddAzureServiceBusTopic(config.EndPoint, config.Topic);

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