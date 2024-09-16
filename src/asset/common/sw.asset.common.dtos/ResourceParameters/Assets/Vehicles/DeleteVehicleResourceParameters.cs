using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Assets.Vehicles
{
    public class DeleteVehicleResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
