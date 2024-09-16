using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sw.auth.common.dtos.Vms.Members.MemberDepartments;
using sw.infrastructure.DTOs.Base;

namespace sw.auth.common.dtos.Vms.Members
{
    public class MemberUiModel : IUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Firstname { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Lastname { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Description { get; set; }
        [Editable(true)]
        public long Id { get; set; }

        [Editable(true)]
        public List<MemberDepartmentUiModel> Departments { get; set; }

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