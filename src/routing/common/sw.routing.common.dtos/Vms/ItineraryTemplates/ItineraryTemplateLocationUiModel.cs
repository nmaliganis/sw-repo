using System;
using System.ComponentModel.DataAnnotations;
using sw.routing.common.dtos.Vms.Locations;
using sw.infrastructure.DTOs.Base;

namespace sw.routing.common.dtos.Vms.ItineraryTemplates
{
    public class ItineraryTemplateLocationUiModel : IUiModel
    {
        [Required]
        [Editable(true)]
        public bool IsStart { get; set; }
        [Required]
        [Editable(true)]
        public LocationUiModel Location { get; set; }
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
}