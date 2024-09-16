using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Accounts;

public class DepartmentRoleForRegistrationUiModel
{
    [Required]
    [Editable(true)]
    public long DepartmentId { get; set; }
}