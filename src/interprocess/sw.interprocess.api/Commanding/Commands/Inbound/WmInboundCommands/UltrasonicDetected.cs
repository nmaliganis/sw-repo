using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using sw.interprocess.api.Commanding.Events.Inbound;
using System.Collections.Generic;
using sw.azure.messaging.Models.IoT;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands
{
  internal class UltrasonicDetected : WmInboundCommand
  {
    private readonly List<IoTUltrasonic> _payload;
    private readonly string _imei;

    public UltrasonicDetected(List<IoTUltrasonic> payload, string imei)
    {
      _payload = payload;
      _imei = imei;
      EventRaisingBehavior = new UltrasonicDetectionEventRaising(_payload, imei);
    }
  }
}