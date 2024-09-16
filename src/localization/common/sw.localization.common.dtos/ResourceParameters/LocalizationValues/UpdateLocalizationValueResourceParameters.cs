using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.ResourceParameters.LocalizationValues
{
    public class UpdateLocalizationValueResourceParameters
    {
        [Required]
        [StringLength(500)]
        public string Value { get; set; }
    }
}
