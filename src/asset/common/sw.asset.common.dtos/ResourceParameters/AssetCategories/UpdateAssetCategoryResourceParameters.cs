using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.AssetCategories
{
    public class UpdateAssetCategoryResourceParameters
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string CodeErp { get; set; }

        [Required]
        public string Params { get; set; }
    }
}
