using System;

namespace sw.auth.messaging.Events
{
    public record AuthRegistration
    {
        public Guid CorrelationId { get; init; }
        public DateTimeOffset Timestamp { get; init; }
    }
}