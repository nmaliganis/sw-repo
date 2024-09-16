using sw.interprocess.api.Commanding.Commands.Base;
using sw.interprocess.api.Commanding.Events.Inbound.Base;
using sw.interprocess.api.Commanding.Servers.Base;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base
{
    public abstract class WmInboundCommand : WmCommand
    {
        public IWmInboundEventRaisingBehavior EventRaisingBehavior { get; set; }

        public void RaiseWmEvent(WmInboundBaseServer inboundEventServer)
        {
            EventRaisingBehavior.RaiseWmEvent(inboundEventServer);
        }
    }
}