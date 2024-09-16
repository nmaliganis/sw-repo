using sw.azure.messaging.Commanding;
using sw.azure.messaging.Events;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace sw.azure.messaging.Consumers
{
    public class IoTMessageConsumer : IConsumer<IoTMessageReceived>
    {
        public async Task Consume(ConsumeContext<IoTMessageReceived> context)
        {
            await Console.Out.WriteLineAsync($"IoT Message with: {context.CorrelationId} for Device : {context.Message.Imei}");
            if (context.Message.Title.Contains("GPS"))
            {
                IoTMessageCommander.Instance.RaiseGpsNotification(true, context.Message.Imei,
                  context.Message.Timestamp, context.Message.PayloadGps);
            }
            else if (context.Message.Title.Contains("Ultrasonic"))
            {
                IoTMessageCommander.Instance.RaiseUltrasonicNotification(true, context.Message.Imei,
                  context.Message.Timestamp, context.Message.PayloadUltrasonic);
            }
            else
            {
                IoTMessageCommander.Instance.RaiseDigitalNotification(true, context.Message.Imei,
          context.Message.Timestamp, context.Message.PayloadDigital);
            }
        }
    }
}