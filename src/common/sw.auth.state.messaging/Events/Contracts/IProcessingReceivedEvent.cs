using System;

namespace sw.auth.state.messaging.Events.Contracts
{
    public interface IProcessingReceivedEvent
    {
        Guid CorrelationId { get; }
        string Title { get; }
    }
}