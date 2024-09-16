using System;

namespace sw.auth.state.messaging.Events.Contracts
{
    public interface IRejectingReceivedEvent
    {
        Guid CorrelationId { get; }
        string ErrorMessage { get; }
    }
}