using sw.interprocess.api.Commanding.Events.Inbound.Base;
using sw.interprocess.api.Commanding.Servers.Base;
using System.Collections.Generic;
using sw.azure.messaging.Models.IoT;

namespace sw.interprocess.api.Commanding.Events.Inbound
{
  public class UltrasonicDetectionEventRaising : IWmInboundEventRaisingBehavior
  {
    public string Imei { get; private set; }
    public List<IoTUltrasonic> Payload { get; private set; }

    public UltrasonicDetectionEventRaising(List<IoTUltrasonic> payload, string imei)
    {
      this.Imei = imei;
      this.Payload = payload;
    }

    public void RaiseWmEvent(WmInboundBaseServer inboundEventServer)
    {
      inboundEventServer.RaiseUltrasonicDetection(Payload, Imei);
    }
  }
}