using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Companies
{
    public class UpdateCompanyResourceParameters
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string CodeErp { get; set; }

        public string Description { get; set; }
    }
}
