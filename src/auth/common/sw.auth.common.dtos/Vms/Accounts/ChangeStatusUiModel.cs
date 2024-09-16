using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Accounts;

public class ChangeStatusUiModel
{
    [Required] public bool Disabled { get; set; }
} //Class : ChangeStatusUiModel