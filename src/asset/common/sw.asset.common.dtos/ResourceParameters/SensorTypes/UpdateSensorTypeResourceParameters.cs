using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.SensorTypes
{
    public record UpdateSensorTypeResourceParameters
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public bool ShowAtStatus { get; set; }

        [Required]
        public long StatusExpiryMinutes { get; set; }

        [Required]
        public bool ShowOnMap { get; set; }

        [Required]
        public bool ShowAtReport { get; set; }

        [Required]
        public bool ShowAtChart { get; set; }

        [Required]
        public bool ResetValues { get; set; }

        [Required]
        public bool SumValues { get; set; }

        [Required]
        public long Precision { get; set; }

        [StringLength(100)]
        public string Tunit { get; set; }

        [Required]
        public bool CalcPosition { get; set; }

        [Required]
        [StringLength(150)]
        public string CodeErp { get; set; }
        [Required]
        public int SensorTypeIndex { get; set; }
    }
}
