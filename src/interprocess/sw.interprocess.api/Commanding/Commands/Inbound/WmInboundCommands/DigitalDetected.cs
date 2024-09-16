using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using sw.interprocess.api.Commanding.Events.Inbound;
using sw.azure.messaging.Models.IoT;
using System.Collections.Generic;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands
{
    internal class DigitalDetected : WmInboundCommand
  {
    private readonly List<IoTDigitalEvent> _payload;
    private readonly string _imei;

    public DigitalDetected(List<IoTDigitalEvent> payload, string imei)
    {
      _payload = payload;
      _imei = imei;
      EventRaisingBehavior = new DigitalDetectionEventRaising(_payload, imei);
    }
  }
}