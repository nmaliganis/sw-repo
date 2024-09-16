using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Simcards;

public class UpdateSimcardResourceParameters
{
  [Required]
  [StringLength(250)]
  public string Number { get; set; }

  [Required]
  [StringLength(150)]
  public string CodeErp { get; set; }

  public string Notes { get; set; }
}