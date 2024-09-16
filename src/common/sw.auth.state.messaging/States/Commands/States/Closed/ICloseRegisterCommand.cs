using sw.auth.state.messaging.States.Commands.Base;

namespace sw.auth.state.messaging.States.Commands.States.Closed
{
    public interface ICloseRegisterCommand : IRegisterCommand
    {
        long MemberId { get; set; }
    }
}