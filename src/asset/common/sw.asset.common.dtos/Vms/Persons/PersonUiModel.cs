using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Persons;

public class PersonUiModel : IUiModel
{
  [Key] public long Id { get; set; }

  [Required(AllowEmptyStrings = false)]
  [Editable(true)]
  public string Message { get; set; }

  [Required] [Editable(true)] public string Username { get; set; }
  [Required] [Editable(true)] public string Email { get; set; }
}