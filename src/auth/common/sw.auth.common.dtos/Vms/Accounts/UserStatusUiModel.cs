using System;
using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Accounts;

public class UserStatusUiModel
{
    [Required]
    [Editable(true)]
    public Guid Id { get; set; }
    [Required]
    [Editable(true)]
    public bool ChangedStatus { get; set; }
    [Editable(true)]
    public string Message { get; set; }
}