using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.auth.common.dtos.Vms.Companies;

public class CompanyDeletionUiModel : IUiModel {
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