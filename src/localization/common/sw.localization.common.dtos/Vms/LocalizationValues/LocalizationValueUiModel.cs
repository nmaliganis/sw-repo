using sw.localization.common.dtos.Vms.Bases;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace sw.localization.common.dtos.Vms.LocalizationValues
{
    public class LocalizationValueUiModel : IUiModel
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

        public static implicit operator LocalizationValueUiModel(Task<LocalizationValueUiModel> v)
        {
            throw new NotImplementedException();
        }
    }
}