using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Templates;

public class ZoneItineraryTemplateResourceParameters
{
    [Required]
    public long ZoneId { get; set; }
    [Required]
    public List<ContainerItineraryTemplateResourceParameters> Containers { get; set; }
}