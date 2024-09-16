using sw.localization.common.dtos.Vms.Bases;
using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.Vms.LocalizationDomains
{
    public class LocalizationDomainDeletionUiModel : IUiModel
    {
        [Required]
        [Editable(false)]
        public bool Successful { get; set; }

        [Editable(false)]
        public long Id { get; set; }

        [Editable(false)]
        public bool Hard { get; set; }

        public string Message { get; set; }
    }
}