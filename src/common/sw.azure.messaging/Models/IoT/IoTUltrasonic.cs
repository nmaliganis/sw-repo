using System;

namespace sw.azure.messaging.Models.IoT
{
    public class IoTUltrasonic : BaseIoT
    {
        public IoTUltrasonic() { }
        public IoTUltrasonic(string imei, DateTime recorded, double range, double status, double temperature)
        {
            this.Imei = imei;
            this.Recorded = recorded;
            this.Range = range;
            this.Status = status;
            this.Temperature = temperature;
        }

        public double? Level { get; set; }
        public double? Range { get; set; }
        public double? Status { get; set; }
        public double? Temperature { get; set; }

    }// Class: IoTUltrasonic
}