using sw.infrastructure.CustomTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace sw.asset.common.dtos.ResourceParameters.Sensor
{
    public record CreateSensorByImeiResourceParameters
    {
        [Required]
        public string DeviceImei { get; set; }

        [Required]
        public int SensorTypeIndex { get; set; }

        [Required]
        public string Params { get; set; }

        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string CodeErp { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public bool IsVisible { get; set; }

        public long Order { get; set; }

        public double MinValue { get; set; }

        public double MaxValue { get; set; }
        public double MinNotifyValue { get; set; }
        public double MaxNotifyValue { get; set; }
        public double LastValue { get; set; }

        [Required]
        public DateTime LastRecordedDate { get; set; }

        [Required]
        public DateTime LastReceivedDate { get; set; }

        public double HighThreshold { get; set; }
        public double LowThreshold { get; set; }
        public long SamplingInterval { get; set; }
        public long ReportingInterval { get; set; }
    }
}
