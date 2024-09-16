using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.ResourceParameters.LocalizationLanguages
{
    public class CreateLocalizationLanguageResourceParameters
    {
        [Required]
        [StringLength(500)]
        public string Language { get; set; }
    }
}
