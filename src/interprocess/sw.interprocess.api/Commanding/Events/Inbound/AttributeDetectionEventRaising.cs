using sw.interprocess.api.Commanding.Events.Inbound.Base;
using sw.interprocess.api.Commanding.Servers.Base;

namespace sw.interprocess.api.Commanding.Events.Inbound
{
  public class AttributeDetectionEventRaising : IWmInboundEventRaisingBehavior
  {
    public string Imei { get; private set; }
    public string Payload { get; private set; }

    public AttributeDetectionEventRaising(string payload, string imei)
    {
      this.Payload = payload;
    }

    public void RaiseWmEvent(WmInboundBaseServer inboundEventServer)
    {
      inboundEventServer.RaiseAttributeDetection(Payload, Imei);
    }
  }
}