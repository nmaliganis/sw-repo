using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.ResourceParameters.LocalizationLanguages
{
    public class DeleteLocalizationLanguageResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
