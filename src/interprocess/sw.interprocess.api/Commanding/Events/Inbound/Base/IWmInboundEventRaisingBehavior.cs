using sw.interprocess.api.Commanding.Servers.Base;

namespace sw.interprocess.api.Commanding.Events.Inbound.Base

{
  public interface IWmInboundEventRaisingBehavior
  {
    void RaiseWmEvent(WmInboundBaseServer inboundEventServer);
  }
}