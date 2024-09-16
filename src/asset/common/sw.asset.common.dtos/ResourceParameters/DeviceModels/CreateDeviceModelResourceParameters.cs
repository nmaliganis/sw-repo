using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.DeviceModels
{
    public class CreateDeviceModelResourceParameters
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string CodeName { get; set; }

        [Required]
        [StringLength(150)]
        public string CodeErp { get; set; }

        [Required]
        public bool Enabled{ get; set; }
    }
}
