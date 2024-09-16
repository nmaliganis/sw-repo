using System;

namespace sw.azure.messaging.Models.IoT
{
    public class IoTUltrasonicLevel : BaseIoT
    {
        public IoTUltrasonicLevel(string imei, DateTime recorded, double range, double level, double status, double temperature)
        {
            this.Imei = imei;
            this.Recorded = recorded;
            this.Range = range;
            this.Level = level;
            this.Status = status;
            this.Temperature = temperature;
        }

        public double? Range { get; set; }
        public double? Level { get; set; }
        public double? Status { get; set; }
        public double? Temperature { get; set; }

    }// Class: IoTUltrasonicLevel
}