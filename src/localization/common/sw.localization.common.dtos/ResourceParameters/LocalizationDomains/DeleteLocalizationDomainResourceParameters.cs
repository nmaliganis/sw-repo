using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.ResourceParameters.LocalizationDomains
{
    public class DeleteLocalizationDomainResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
