using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Simcards;

public class SimcardCreationUiModel : IUiModel
{
  [Required]
  [Editable(true)]
  public string Iccid { get; set; }
  [Required]
  [Editable(true)]
  public string Imsi { get; set; }
  [Required]
  [Editable(true)]
  public string CountryIso { get; set; }
  [Required]
  [Editable(true)]
  public string Number { get; set; }
  [Required]
  [Editable(true)]
  public DateTime PurchaseDate { get; set; }
  [Required]
  [Editable(true)]
  public int CardType { get; set; }
  [Required]
  [Editable(true)]
  public int NetworkType { get; set; }
  [Required]
  [Editable(true)]
  public bool IsEnabled { get; set; }

  [Editable(true)]
  public long DeviceId { get; set; }

  [Key]
  public long Id { get; set; }

  [Editable(true)]
  public string Message { get; set; }
}