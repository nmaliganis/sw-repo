using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.routing.common.dtos.Vms.Jobs
{
    public class JobUiModel : IUiModel
    {
        [Editable(true)] public long Container { get; set; }
        [Editable(true)] public DateTime Arrival { get; set; }
        [Editable(true)] public DateTime EstimatedArrival { get; set; }
        [Editable(true)] public DateTime Departure { get; set; }
        [Editable(true)] public DateTime EstimatedDeparture { get; set; }
        [Editable(true)] public DateTime ScheduledArrival { get; set; }
        [Editable(true)] public virtual string Seq { get; set; }

        [Editable(true)] public long Id { get; set; }

        public string Message { get; set; }

        [Required] [Editable(true)] public DateTime CreatedDate { get; set; }
        [Required] [Editable(true)] public DateTime ModifiedDate { get; set; }
        [Required] [Editable(true)] public bool Active { get; set; }
    }
}