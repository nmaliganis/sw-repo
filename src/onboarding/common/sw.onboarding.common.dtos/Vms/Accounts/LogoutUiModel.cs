using System;
using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.Vms.Accounts
{
    public class LogoutUiModel
    {
        [Editable(true)]
        [Required]
        public long UserId { get; set; }
    }
}