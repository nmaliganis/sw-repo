using sw.azure.messaging.Models.IoT;
using System.Collections.Generic;

namespace sw.interprocess.api.Commanding.Events.EventArgs.Inbound
{
    public class DigitalDetectionEventArgs : System.EventArgs
    {
        public List<IoTDigitalEvent> Payload { get; private set; }
        public string Imei { get; private set; }
        public bool Success { get; private set; }

        public DigitalDetectionEventArgs(List<IoTDigitalEvent> payload, bool success, string imei)
        {
            Payload = payload;
            Imei = imei;
            Success = success;
        }
    }
}
