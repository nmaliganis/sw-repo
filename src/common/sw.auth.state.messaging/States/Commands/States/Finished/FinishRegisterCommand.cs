using System;

namespace sw.auth.state.messaging.States.Commands.States.Finished
{
    public class FinishRegisterCommand : IFinishRegisterCommand
    {
        public Guid CorrelationId { get; set; }
        public DateTime Timestamp { get; set; }
        public long MemberId { get; set; }
    }
}