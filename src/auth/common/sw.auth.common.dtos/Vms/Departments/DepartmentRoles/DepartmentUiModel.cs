using System;
using System.ComponentModel.DataAnnotations;
using sw.auth.common.dtos.Vms.Roles;
using sw.common.dtos.Vms.Departments;
using sw.infrastructure.DTOs.Base;

namespace sw.auth.common.dtos.Vms.Departments.DepartmentRoles
{
    public class DepartmentRoleUiModel : IUiModel
    {
        [Editable(true)]
        public long Id { get; set; }

        [Editable(true)]
        public DepartmentUiModel Department { get; set; }

        [Editable(true)]
        public RoleUiModel Role { get; set; }

        public string Message { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Notes { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }

        [Required]
        [Editable(true)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Editable(true)]
        public DateTime ModifiedDate { get; set; }

        [Required]
        [Editable(true)]
        public bool Active { get; set; }
    }
}