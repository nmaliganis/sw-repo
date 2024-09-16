using sw.auth.state.messaging.States.Commands.Base;

namespace sw.auth.state.messaging.States.Commands.States.Finished
{
    public interface IFinishRegisterCommand : IRegisterCommand
    {
        long MemberId { get; set; }
    }
}