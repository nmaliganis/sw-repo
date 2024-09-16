using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sw.routing.common.dtos.Vms.DriversTransportCombinations;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.common.dtos.Vms.Jobs;
using sw.infrastructure.DTOs.Base;

namespace sw.routing.common.dtos.Vms.Itineraries;

public class ItineraryUiModel : IUiModel
{
    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public string Name { get; set; }
    [Editable(true)]
    public ItineraryTemplateUiModel Template { get; set; }
    [Editable(true)]
    public DriverTransportCombinationUiModel DriverTransportCombination { get; set; }
    [Editable(true)]
    public virtual List<JobUiModel> Jobs { get; set; }

    [Editable(true)]
    public long Id { get; set; }

    public string Message { get; set; }

    [Required]
    [Editable(true)]
    public DateTime CreatedDate { get; set; }
    [Required]
    [Editable(true)]
    public DateTime ModifiedDate { get; set; }
    [Required]
    [Editable(true)]
    public bool Active { get; set; }
}