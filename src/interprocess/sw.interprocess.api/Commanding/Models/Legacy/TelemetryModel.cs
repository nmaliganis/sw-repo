using System;
using sw.interprocess.api.Commanding.Models.Base;

namespace sw.interprocess.api.Commanding.Models.Legacy
{
    public class TelemetryModel : BaseModel
    {
        public DateTime PayloadTimestamp { get; set; }
        public byte PayloadTimestampValue { get; set; }
        public string PayloadBattery { get; set; }
        public byte PayloadBatteryValue { get; set; }
        public string PayloadTemp { get; set; }
        public byte[] PayloadTempValue { get; set; }
        public double PayloadLatitude { get; set; }
        public byte[] PayloadLatitudeValue { get; set; }
        public string PayloadAltitude { get; set; }
        public byte[] PayloadAltitudeValue { get; set; }
        public string PayloadSpeed { get; set; }
        public byte[] PayloadSpeedValue { get; set; }
        public string PayloadCourse { get; set; }
        public byte PayloadCourseValue { get; set; }
        public string PayloadSatellites { get; set; }
        public byte PayloadSatellitesValue { get; set; }
        public string PayloadTimeToFix { get; set; }
        public byte PayloadTimeToFixValue { get; set; }
        public string PayloadMeasuredDistance { get; set; }
        public byte[] PayloadMeasuredDistanceValue { get; set; }
        public string PayloadFillLevel { get; set; }
        public byte PayloadFillLevelValue { get; set; }
        public string PayloadSignal { get; set; }
        public byte PayloadSignalValue { get; set; }
        public int PayloadStatusFlag { get; set; }
        public byte PayloadStatusFlagValue { get; set; }
        public int PayloadResetCause { get; set; }
        public byte PayloadResetCauseValue { get; set; }
        public string PayloadFwVer { get; set; }
        public byte[] PayloadFwVerValue { get; set; }
    }
}
