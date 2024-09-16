using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.ResourceParameters.LocalizationValues
{
    public class CreateLocalizationValueResourceParameters
    {
        [Required]
        [StringLength(500)]
        public string Domain { get; set; }

        [Required]
        [StringLength(500)]
        public string Language { get; set; }

        [Required]
        [StringLength(500)]
        public string Key { get; set; }

        [Required]
        [StringLength(500)]
        public string Value { get; set; }
    }
}
