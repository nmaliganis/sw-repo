using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.ResourceParameters.LocalizationDomains
{
    public class CreateLocalizationDomainResourceParameters
    {
        [Required]
        [StringLength(500)]
        public string Domain { get; set; }
    }
}
