using System;

namespace sw.azure.messaging.Events
{
    public class ParsingFailureReceived
    {
        public Guid CorrelationId { get; set; }
        public DateTime Timestamp { get; set; }
        public byte[] BinaryMessage { get; set; }
    }
}