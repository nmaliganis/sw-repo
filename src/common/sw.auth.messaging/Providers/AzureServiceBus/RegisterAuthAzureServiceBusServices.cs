using System;
using Azure.Messaging.ServiceBus;
using sw.auth.messaging.Configurations;
using sw.auth.messaging.Consumers;
using sw.auth.messaging.Events;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sw.auth.messaging.Providers.AzureServiceBus;

public static class RegisterAuthAzureServiceBusServices
{
  public static IServiceCollection RegisterAzureServiceBusAsConsumerServices(this IServiceCollection services, IConfiguration configuration)
  {

    var config = ConfigureBus(configuration);

    services.AddMassTransit(x =>
    {
      x.AddServiceBusMessageScheduler();
      x.SetKebabCaseEndpointNameFormatter();
      x.AddConsumer<AuthRegisterConsumer>();

      x.UsingAzureServiceBus((_, cfg) =>
      {
        cfg.Host(config.EndPoint, h =>
        {
          h.RetryLimit = 1;
          h.TransportType = ServiceBusTransportType.AmqpTcp;
        });
        cfg.UseServiceBusMessageScheduler();

        cfg.Send<AuthRegistration>(s
          => s.UseSessionIdFormatter(c => c.Message.CorrelationId.ToString("D")));

        cfg.SubscriptionEndpoint<AuthRegistration>($"{config.Topic}-consumer", e =>
        {
          e.ConfigureConsumer<AuthRegisterConsumer>(_);
        });

        cfg.ConfigureEndpoints(_);
      });
    });

    services.AddMassTransitHostedService(true);

    return services;
  }

  public static IServiceCollection RegisterAzureServiceBusAsPublisherServices(this IServiceCollection services, IConfiguration configuration)
  {

    var config = ConfigureBus(configuration);

    services.AddMassTransit(x =>
    {

      x.AddServiceBusMessageScheduler();
      x.SetKebabCaseEndpointNameFormatter();
      //x.AddConsumer<AuthRegisterConsumer>();

      x.UsingAzureServiceBus((_, cfg) =>
      {
        cfg.Host(config.EndPoint, h =>
        {
          h.RetryLimit = 1;
          h.TransportType = ServiceBusTransportType.AmqpTcp;
        });
        cfg.UseServiceBusMessageScheduler();

        cfg.Send<AuthRegistration>(s
          => s.UseSessionIdFormatter(c => c.Message.CorrelationId.ToString("D")));
      });
    });

    services.AddMassTransitHostedService();

    return services;
  }


  private static ServiceBusConfig ConfigureBus(IConfiguration configuration)
  {
    ServiceBusConfig config = new ServiceBusConfig()
    {
      EndPoint = configuration.GetSection("AzureServiceBus:EndPoint").Value,
      SasLocator = configuration.GetSection("AzureServiceBus:SasLocator").Value,
      Topic = configuration.GetSection("AzureServiceBus:Topic-Auth").Value,
    };
    return config;
  }
}