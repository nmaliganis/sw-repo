using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.admin.common.dtos.V1.Vms.Departments
{
    public class DepartmentCreationUiModel : IUiModel
    {
        [Key]
        [Editable(true)]
        public long Id { get; set; }

        public string Message { get; set; }
    }
}
