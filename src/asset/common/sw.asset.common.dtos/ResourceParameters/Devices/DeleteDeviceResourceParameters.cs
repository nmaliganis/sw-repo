using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Devices
{
    public class DeleteDeviceResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
