using System;

namespace sw.azure.messaging.Models.IoT
{
    public class IoTDigitalEvent : BaseIoT
    {

        public IoTDigitalEvent(string imei, DateTime recorded, int pinNumber, int newValue)
        {
            Imei = imei;
            Recorded = recorded;
            this.PinNumber = pinNumber;
            this.NewValue = newValue;

        }

        public int? PinNumber { get; set; }
        public int? NewValue { get; set; }
    }// Class: IoTDigitalEvent
}// Namespace: sw.azure.messaging.Models.IoT
