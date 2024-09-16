using System;

namespace sw.auth.state.messaging.States.Commands.States.Registered;

public class StartRegisterCommand : IStartRegisterCommand
{
  public string MemberFirstname { get; set; }
  public string MemberLastname { get; set; }
  public long MemberAuthId { get; set; }
  public string Title { get; set; }
  public Guid CorrelationId { get; set; }
  public DateTime Timestamp { get; set; }
}