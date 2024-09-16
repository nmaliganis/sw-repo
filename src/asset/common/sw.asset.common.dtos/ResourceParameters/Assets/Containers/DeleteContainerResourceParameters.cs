using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Assets.Containers
{
    public class DeleteContainerResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
