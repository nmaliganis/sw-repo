using sw.localization.common.dtos.Vms.Bases;
using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.Vms.LocalizationLanguages
{
    public class LocalizationLanguageUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Domain { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Language { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Key { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Value { get; set; }
    }
}