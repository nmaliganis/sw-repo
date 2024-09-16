using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.admin.common.dtos.V1.Vms.Departments
{
    public class DepartmentModificationUiModel : IUiModel
    {
        public string Message { get; set; }

        [Key]
        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }

        [Editable(true)]
        public string Notes { get; set; }

        [Required()]
        [Editable(true)]
        public long CompanyId { get; set; }
    }
}
