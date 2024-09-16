using System;

namespace sw.auth.state.messaging.Events.Contracts
{
    public interface IRegisteringReceivedEvent
    {
        Guid CorrelationId { get; }
        string Title { get; }
    }
}