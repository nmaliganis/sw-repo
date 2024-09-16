using System;
using sw.interprocess.api.Commanding.Models.Base;

namespace sw.interprocess.api.Commanding.Models.Rows
{
    public class TelemetryRow : BaseModel
    {
        public string CommandType { get; set; }
        public double Temperature { get; set; }
        public double FillLevel { get; set; }
        public double TiltX { get; set; }
        public double TiltY { get; set; }
        public double TiltZ { get; set; }
        public decimal Light { get; set; }
        public double Battery { get; set; }
        public string Gps { get; set; }
        public string NbIoT { get; set; }
        public double Distance { get; set; }
        public decimal Tamper { get; set; }
        public decimal NbIoTSignalLength { get; set; }


        public string LatestResetCause { get; set; }
        public string FirmwareVersion { get; set; }


        public bool TemperatureEnable { get; set; }
        public bool DistanceEnable { get; set; }
        public bool TiltEnable { get; set; }
        public bool MagnetometerEnable { get; set; }
        public bool TamperEnable { get; set; }
        public bool LightEnable { get; set; }
        public bool GpsEnable { get; set; }

        public decimal BatterySafeMode { get; set; }
        public decimal NbIoTMode { get; set; }


        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Speed { get; set; }
        public decimal Bearing { get; set; }
        public decimal Angle { get; set; }
        public int NumOfSatellites { get; set; }
        public decimal TimeToFix { get; set; }
        public decimal SignalLength { get; set; }
        public decimal StatusFlags { get; set; }
    }
}