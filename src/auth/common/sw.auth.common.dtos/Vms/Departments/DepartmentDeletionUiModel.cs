using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Departments;

public class DepartmentDeletionUiModel {
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