using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Simcards;

public class SimcardModificationUiModel : IUiModel
{
  [Required] [Editable(true)] public string SimcardName { get; set; }
  [Required] [Editable(true)] public string SimcardCodeName { get; set; }

  [Key] public long Id { get; set; }
  public string Message { get; set; }
}