using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.auth.common.dtos.Vms.Roles
{
    public class RoleUiModel : IUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }
        [Editable(true)]
        public long Id { get; set; }

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