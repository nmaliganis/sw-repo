using System;
using sw.auth.state.messaging.States.Commands.States.Closed;

namespace sw.auth.messaging.States.Commands.States.Closed
{
    public class CloseRegisterCommand : ICloseRegisterCommand
    {
        public Guid CorrelationId { get; set; }
        public DateTime Timestamp { get; set; }
        public long MemberId { get; set; }
    }
}