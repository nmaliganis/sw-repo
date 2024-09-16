using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Simcards;

public class SimcardUiModel : IUiModel
{
  [Key]
  public long Id { get; set; }

  public string Message { get; set; }
    
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
  public DateTime CreatedDate { get; set; }
  [Required]
  [Editable(true)]
  public int Type { get; set; }
  [Required]
  [Editable(true)]
  public string TypeValue { get; set; }
  [Required]
  [Editable(true)]
  public int NetworkType { get; set; }
  [Required]
  [Editable(true)]
  public string NetworkTypeValue { get; set; }
  [Required]
  [Editable(true)]
  public bool IsEnabled { get; set; }
  [Editable(true)]
  public long DeviceId { get; set; }
}