using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.SensorTypes
{
    public record DeleteSensorTypeResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
