using sw.interprocess.api.Commanding.Events.Inbound.Base;
using sw.interprocess.api.Commanding.Servers.Base;
using sw.azure.messaging.Models.IoT;
using System.Collections.Generic;

namespace sw.interprocess.api.Commanding.Events.Inbound
{
    public class DigitalDetectionEventRaising : IWmInboundEventRaisingBehavior
    {
        public string Imei { get; private set; }
        public List<IoTDigitalEvent> Payload { get; private set; }

        public DigitalDetectionEventRaising(List<IoTDigitalEvent> payload, string imei)
        {
            this.Imei = imei;
            this.Payload = payload;
        }

        public void RaiseWmEvent(WmInboundBaseServer inboundEventServer)
        {
            inboundEventServer.RaiseDigitalDetection(Payload, Imei);
        }
    }
}