using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Simcards;

public class SimcardDeletionUiModel : IUiModel
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