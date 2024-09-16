using System;
using System.ComponentModel.DataAnnotations;
using sw.auth.common.dtos.Vms.Companies;
using sw.auth.common.dtos.Vms.Departments;
using sw.common.dtos.Vms.Departments;
using sw.infrastructure.DTOs.Base;

namespace sw.auth.common.dtos.Vms.Members.MemberDepartments
{
    public class MemberDepartmentUiModel : IUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Description { get; set; }
        [Editable(true)]
        public long Id { get; set; }

        [Required]
        [Editable(true)]
        public MemberUiModel Member { get; set; }

        [Required]
        [Editable(true)]
        public DepartmentUiModel Department { get; set; }

        public string Message { get; set; }

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