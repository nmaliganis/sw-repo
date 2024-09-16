using System;

namespace sw.auth.state.messaging.States.Commands.States.Processed
{
    public class ProcessRegisterCommand : IProcessRegisterCommand
    {
        public Guid CorrelationId { get; set; }
        public DateTime Timestamp { get; set; }
        public string MemberStatus { get; set; }
    }
}