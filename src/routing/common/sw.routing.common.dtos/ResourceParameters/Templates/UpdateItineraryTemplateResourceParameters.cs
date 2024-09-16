using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Templates;

public class UpdateItineraryTemplateResourceParameters
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ZoneItineraryTemplateResourceParameters Zones { get; set; }
    public List<int> Occurrence { get; set; }

}// Class: UpdateItineraryTemplateResourceParameters