using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Simcards;

public class DeleteSimcardResourceParameters
{
  [Required]
  public string DeletedReason { get; set; }
}