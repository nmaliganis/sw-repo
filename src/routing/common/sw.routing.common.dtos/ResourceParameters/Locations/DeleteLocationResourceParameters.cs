using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Locations;

public class DeleteLocationResourceParameters
{
    [Required]
    public string DeletedReason { get; set; }
}