using sw.auth.state.messaging.States.Commands.Base;

namespace sw.auth.state.messaging.States.Commands.States.Rejected;

public interface IRejectRegisterCommand : IRegisterCommand
{
  string MemberErrorMessage { get; set; }
}