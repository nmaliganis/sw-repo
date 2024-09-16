using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Itineraries;

public class ItineraryContainerResourceParameters
{
    [Key]
    [Required]
    public long Id { get; set; }
    [Required]
    public DateTime ScheduledArrival { get; set; }
    [Required]
    public List<long> Seq { get; set; }
}


public class VehicleResourceParameters
{
    [Key]
    [Required]
    public long Id { get; set; }
    [Required]
    public int Type { get; set; }
}