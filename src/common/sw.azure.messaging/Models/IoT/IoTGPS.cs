using System;

namespace sw.azure.messaging.Models.IoT
{
    public class IoTgps :BaseIoT
    {
        public IoTgps(string imei, DateTime recorded, double latitude, double longitude, double altitude, double speed, double direction, int fixMode, double hdop, int satellitesUsed)
        {
            this.Imei = imei;
            this.Recorded = recorded;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Altitude = altitude;
            this.Speed = speed;
            this.Direction = direction;
            this.FixMode = fixMode;
            this.Hdop = hdop;
            this.SatellitesUsed = satellitesUsed;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public int FixMode { get; set; }
        public double Hdop { get; set; }
        public int SatellitesUsed { get; set; }

    }// Class: IoTGPS
}