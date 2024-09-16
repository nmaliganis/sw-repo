using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.AssetCategories
{
    public class DeleteAssetCategoryResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
