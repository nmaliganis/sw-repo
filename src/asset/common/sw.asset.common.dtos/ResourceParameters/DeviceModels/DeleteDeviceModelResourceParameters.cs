using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.DeviceModels
{
    public class DeleteDeviceModelResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
