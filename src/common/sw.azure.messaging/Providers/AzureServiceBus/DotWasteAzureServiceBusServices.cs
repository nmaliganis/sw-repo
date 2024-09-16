using System;
using Azure.Messaging.ServiceBus;
using sw.azure.messaging.Busses;
using sw.azure.messaging.Configurations;
using sw.azure.messaging.Consumers;
using sw.azure.messaging.Events;
using MassTransit;
using MassTransit.MultiBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sw.azure.messaging.Providers.AzureServiceBus
{
    public static class swAzureServiceBusServices
    {
        public static IServiceCollection RegisterAsbAsConsumerServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var config = ConfigureBus(configuration);

            services.AddMassTransit(x =>
            {
                x.AddServiceBusMessageScheduler();
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<IoTMessageConsumer>();

                x.UsingAzureServiceBus((_, cfg) =>
                {
                    cfg.Host(config.EndPoint, h =>
                    {
                        h.RetryLimit = 3;
                        h.TransportType = ServiceBusTransportType.AmqpTcp;
                    });
                    cfg.UseServiceBusMessageScheduler();

                    cfg.Send<IoTMessageReceived>(s
                        => s.UseSessionIdFormatter(c => c.Message.CorrelationId.ToString("D")));

                    cfg.SubscriptionEndpoint<IoTMessageReceived>($"{config.Topic}-consumer",
                        e => { e.ConfigureConsumer<IoTMessageConsumer>(_); });

                    cfg.ConfigureEndpoints(_);
                });
            });

            services.AddMassTransitHostedService(true);
            services.AddHealthChecks().AddAzureServiceBusTopic(config.EndPoint, config.Topic);

            return services;
        }

        public static IServiceCollection RegisterAsbAsPublisherServices(this IServiceCollection services,
            IConfiguration configuration)
        {
             var config = ConfigureBus(configuration);

            if (config.Topic != null)
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

                        cfg.Send<IoTMessageReceived>(s
                            => s.UseSessionIdFormatter(c => c.Message.CorrelationId.ToString("D")));
                    });
                });
            }

            if (config.TopicDriver != null)
            {
                services.AddMassTransit<IDriverCreationBus>(x =>
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

            if (config.TopicError != null)
            {
                services.AddMassTransit<IFailureBus>(x =>
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

                        cfg.Send<ParsingFailureReceived>(s
                            => s.UseSessionIdFormatter(c => c.Message.CorrelationId.ToString("D")));
                    });
                });
            }


            services.AddMassTransitHostedService();
            return services;
        }

        private static AzureServiceBusConfig ConfigureBus(IConfiguration configuration)
        {
            AzureServiceBusConfig config = new AzureServiceBusConfig()
            {
                EndPoint = configuration.GetSection("ASB:EndPoint")!.Value,
                SasLocator = configuration.GetSection("ASB:SasLocator")!.Value,
                Topic = configuration.GetSection("ASB:Topic")!.Value,
                TopicDriver = configuration.GetSection("ASB:TopicDriver")!.Value,
                TopicError = configuration.GetSection("ASB:TopicError")!.Value,
            };
            return config;
        }
    }
}