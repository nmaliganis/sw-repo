using sw.infrastructure.CustomTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Templates;

public class CreateItineraryTemplateResourceParameters
{
    [Required]
    public string Name { get; set; }
    [Required] 
    public int Stream { get; set; }
    [Required]
    public long Zone { get; set; }
    [Required]
    public List<int> Occurrence { get; set; }
    [Required]
    public long StartFrom { get; set; }
    [Required]
    public long EndTo { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double MinFillLevel { get; set; }
    [Required]
    public string StartTime { get; set; }
}