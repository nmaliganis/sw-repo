using System;
using System.Threading.Tasks;
using sw.auth.messaging.Commanding;
using sw.auth.messaging.Events;
using MassTransit;

namespace sw.auth.messaging.Consumers
{
    public class AuthRegisterConsumer : IConsumer<AuthRegistration>
    {
        public async Task Consume(ConsumeContext<AuthRegistration> context)
        {
            await Console.Out.WriteLineAsync($"Auth Register with: {context.CorrelationId}");
            MessageCommander.Instance.RaiseModalNotification(true);
        }
    }
}