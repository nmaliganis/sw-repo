using System.Collections.Generic;
using sw.azure.messaging.Models.IoT;

namespace sw.interprocess.api.Commanding.Events.EventArgs.Inbound
{
    public class GPSDetectionEventArgs : System.EventArgs
    {
        public IoTgps Payload { get; private set; }
        public string Imei { get; private set; }
        public bool Success { get; private set; }

        public GPSDetectionEventArgs(IoTgps payload, bool success, string imei)
        {
            Payload = payload;
            Imei = imei;
            Success = success;
        }
    }
}
