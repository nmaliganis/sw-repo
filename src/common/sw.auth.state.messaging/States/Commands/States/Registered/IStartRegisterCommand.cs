using sw.auth.state.messaging.States.Commands.Base;

namespace sw.auth.state.messaging.States.Commands.States.Registered;

public interface IStartRegisterCommand : IRegisterCommand
{
  string MemberFirstname { get; set; }
  string MemberLastname { get; set; }
  public long MemberAuthId { get; set; }
  string Title { get; set; }
}