using System;

namespace sw.auth.state.messaging.States.Commands.States.Rejected;

public class RejectRegisterCommand : IRejectRegisterCommand
{
  public Guid CorrelationId { get; set; }
  public DateTime Timestamp { get; set; }
  public string MemberErrorMessage { get; set; }
}