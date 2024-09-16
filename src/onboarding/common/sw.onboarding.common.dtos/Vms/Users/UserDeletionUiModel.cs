using System;
using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.Vms.Users
{
    public class UserDeletionUiModel
    {
        [Required]
        [Editable(true)]
        public long Id { get; set; }
        [Required]
        [Editable(true)]
        public bool Active { get; set; }
        [Required]
        [Editable(true)]
        public bool DeletionStatus { get; set; }
        [Required]
        [Editable(true)]
        public string Message { get; set; }
    }
}