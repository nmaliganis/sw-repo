using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Events;

public record DeleteEventHistoryResourceParameters
{
    [Required]
    public string DeletedReason { get; set; }
}
