using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Sensors
{
    public record DeleteSensorResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
