using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.ResourceParameters.LocalizationValues
{
    public class DeleteLocalizationValueResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
