using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sw.auth.common.dtos.Vms.Members;
using sw.auth.common.dtos.Vms.Roles;
using sw.infrastructure.DTOs.Base;

namespace sw.auth.common.dtos.Vms.Users {
    public class UserUiModel : IUiModel {
        [Key]
        public long Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Message { get; set; }

        [Required]
        [Editable(true)]
        public string Login { get; set; }
        [Required]
        [Editable(true)]
        public bool IsActivated { get; set; }
        [Required]
        [Editable(true)]
        public DateTime ResetDate { get; set; }
        public Guid ResetKey { get; set; }
        [Required]
        [Editable(true)]
        public Guid ActivationKey { get; set; }
        public DateTime LastLogin { get; set; }
        [Required]
        [Editable(true)]
        public bool IsLoggedIn { get; set; }
        public bool Disabled { get; set; }
        [Required]
        [Editable(true)]
        public bool Active { get; set; }

        [Required]
        [Editable(true)]
        public long CompanyId { get; set; }

        [Required]
        [Editable(true)]
        public MemberUiModel Member { get; set; }

        [Required]
        [Editable(true)]
        public long MemberId { get; set; }

        [Required] [Editable(true)] public List<long> DepartmentIds { get; set; } = new List<long>();

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string MemberEmail { get; set; }

        [Required] [Editable(true)] public List<RoleUiModel> Roles { get; set; } = new List<RoleUiModel>();
    }
}