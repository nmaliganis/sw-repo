using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.routing.common.dtos.ResourceParameters.Itineraries;

public class CreateItineraryResourceParameters
{
    [Required]
    public string Name { get; set; }

    [Required]
    public int MinTimePerBin { get; set; }
    [Required]
    public int MinCollectionTimePerBin { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }


    //Dependencies
    [Required]
    public List<VehicleResourceParameters> Vehicles { get; set; }
    [Required]
    public List<ItineraryContainerResourceParameters> Containers { get; set; }
    [Required]
    public long DriverId { get; set; }
    [Required]
    public long ItineraryTemplateId { get; set; }
    public long CorrelationItineraryId { get; set; }
}