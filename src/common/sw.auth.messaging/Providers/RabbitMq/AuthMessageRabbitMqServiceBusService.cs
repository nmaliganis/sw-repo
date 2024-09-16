using System;
using System.Threading;
using sw.auth.messaging.Configurations;
using sw.auth.messaging.Consumers;
using GreenPipes;
using MassTransit;
using MassTransit.Util;

namespace sw.auth.messaging.Providers.RabbitMq
{
    public class AuthMessageRabbitMqServiceBusService : IAuthMessageServiceBus
    {
        private IBusControl _busControl;


        private void Initialization()
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(RabbitMqConstants.RabbitMqUri, "auth", h =>
                {
                    h.Username(RabbitMqConstants.UserName);
                    h.Password(RabbitMqConstants.Password);
                });

                cfg.ReceiveEndpoint(RabbitMqConstants.AuthDotTrackServiceQueue, e =>
                {
                    e.PrefetchCount = 100;
                    e.UseRateLimit(1, TimeSpan.FromSeconds(15));

                    e.UseRetry(r => {
                        r.Immediate(10);
                    });

                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TripThreshold = 15;
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.ActiveThreshold = 10;
                    });

                    e.Consumer(() => new AuthRegisterConsumer());
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        }

        public void CreatePublisher(ServiceBusConfig config)
        {
            Initialization();
        }

        public void CreateSubscriber(ServiceBusConfig config)
        {
            Initialization();
        }

        void IAuthMessageServiceBus.StopBus()
        {
            TaskUtil.Await(() => _busControl.StartAsync());
        }

        void IAuthMessageServiceBus.StartBus()
        {
            TaskUtil.Await(() => _busControl.StopAsync());
        }
    }
}