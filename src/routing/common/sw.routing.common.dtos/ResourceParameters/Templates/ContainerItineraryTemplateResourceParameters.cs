using System;
using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Templates;

public class ContainerItineraryTemplateResourceParameters
{
    [Required]
    public long Id { get; set; }

    [Required]
    public long Level { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required]
    public DateTime LastServicedDate { get; set; }

    [Required]
    public int Status { get; set; }
}