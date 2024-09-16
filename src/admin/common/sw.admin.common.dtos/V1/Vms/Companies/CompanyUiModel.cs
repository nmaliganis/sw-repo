using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.admin.common.dtos.V1.Vms.Companies
{
    public class CompanyUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }

        [Editable(true)]
        public string Description { get; set; }
    }
}
