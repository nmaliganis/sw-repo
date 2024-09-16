using sw.localization.common.dtos.Vms.Bases;
using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.Vms.LocalizationLanguages
{
    public class LocalizationLanguageCreationUiModel : IUiModel
    {
        [Key]
        [Editable(true)]
        public long Id { get; set; }

        public string Message { get; set; }
    }
}