using System;

namespace sw.auth.state.messaging.Events.Contracts
{
    public interface IFinishingReceivedEvent
    {
        Guid CorrelationId { get; }
        string Title { get; }
    }
}