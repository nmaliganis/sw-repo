using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.Vms.LocalizationValues
{
    public class LocalizationValueModificationUiModel
    {
        [Key]
        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Key { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string OldValue { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string NewValue { get; set; }

        public string Message { get; set; }
    }
}