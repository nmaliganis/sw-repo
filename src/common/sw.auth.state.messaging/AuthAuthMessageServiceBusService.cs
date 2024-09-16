using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.state.messaging.States.Sagas;
using sw.auth.state.messaging.States.StateMachines;
using GreenPipes;
using MassTransit;
using MassTransit.Saga;

namespace sw.auth.state.messaging
{
    public class AuthAuthMessageServiceBusService : IAuthMessageServiceBus
    {
        private IBusControl _busControl;
        private ISagaRepository<AuthRecognitionSagaState> _authSagaRepository;

        public AuthAuthMessageServiceBusService()
        {
            Initialization();
        }

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
                    //e.Consumer<AuthRegisteringReceivedConsumer>();

                    e.UseRetry(r => {
                        r.Immediate(10);
                    });

                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TripThreshold = 15;
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.ActiveThreshold = 10;
                    });

                    e.StateMachineSaga(AuthRecognitionStateMachine.StateMachine, this._authSagaRepository);
                    //e.Consumer(() => new AuthRegisteringReceivedConsumer());
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        }

        public async Task StopBus()
        {
           await _busControl.StartAsync();
        }

        public async Task StartBus()
        {
           await _busControl.StopAsync();
        }
    }
}