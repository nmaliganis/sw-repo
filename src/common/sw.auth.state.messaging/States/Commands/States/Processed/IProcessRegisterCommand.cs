using sw.auth.state.messaging.States.Commands.Base;

namespace sw.auth.state.messaging.States.Commands.States.Processed
{
    public interface IProcessRegisterCommand : IRegisterCommand
    {
        string MemberStatus { get; set; }
    }
}