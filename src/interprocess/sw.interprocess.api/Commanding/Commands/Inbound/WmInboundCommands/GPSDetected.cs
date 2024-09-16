using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using sw.interprocess.api.Commanding.Events.Inbound;
using System.Collections.Generic;
using sw.azure.messaging.Models.IoT;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands
{
  internal class GPSDetected : WmInboundCommand
  {
    private readonly IoTgps _payload;
    private readonly string _imei;

    public GPSDetected(IoTgps payload, string imei)
    {
      _payload = payload;
      _imei = imei;
      EventRaisingBehavior = new GpsDetectionEventRaising(_payload, imei);
    }
  }
}