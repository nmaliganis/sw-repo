using sw.interprocess.api.Commanding.Events.Inbound.Base;
using sw.interprocess.api.Commanding.Servers.Base;

namespace sw.interprocess.api.Commanding.Events.Inbound
{
  public class TelemetryDetectionEventRaising : IWmInboundEventRaisingBehavior
  {
    public string Payload { get; private set; }
    public string Imei { get; private set; }

    public TelemetryDetectionEventRaising(string payload, string imei)
    {
      this.Payload = payload;
      this.Imei = imei;
    }

    public void RaiseWmEvent(WmInboundBaseServer inboundEventServer)
    {
      inboundEventServer.RaiseTelemetryDetection(Payload, Imei);
    }
  }
}