using System;
using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Accounts;

public class AccountForActivationModification
{
    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public Guid ActivationCode { get; set; }
}