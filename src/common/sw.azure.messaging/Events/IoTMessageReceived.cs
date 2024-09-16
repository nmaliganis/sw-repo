using sw.azure.messaging.Models.IoT;
using System;
using System.Collections.Generic;

namespace sw.azure.messaging.Events
{
    public class IoTMessageReceived
    {
        public Guid CorrelationId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Imei { get; set; }
        public string Title { get; set; }
        public List<IoTUltrasonic> PayloadUltrasonic { get; set; }
        public IoTgps PayloadGps { get; set; }
        public List<IoTDigitalEvent> PayloadDigital { get; set; }
    }
}