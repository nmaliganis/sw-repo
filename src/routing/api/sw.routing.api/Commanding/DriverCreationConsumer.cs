using System;
using System.Threading.Tasks;
using MassTransit;
using Serilog;

namespace sw.routing.api.Commanding;

public class DriverCreationConsumer : IConsumer<DriverCreationReceived>
{
    public async Task Consume(ConsumeContext<DriverCreationReceived> context)
    {
        Log.Information($"Driver Creation with: {context.CorrelationId}");
    }
}