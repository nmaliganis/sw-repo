using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.CustomTypes;
using sw.infrastructure.DTOs.Base;

namespace sw.routing.common.dtos.Vms.ItineraryTemplates
{
    public class ItineraryTemplateUiModel : IUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }
        [Editable(true)]
        public long Id { get; set; }
        [Required]
        [Editable(true)]
        public List<ItineraryTemplateLocationUiModel> Locations { get; set; }

        public string Description { get; set; }
        public double MinFillLevel { get; set; }
        public string StartTime { get; set; }

        public string Stream { get; set; }
        public string Zones { get; set; }
        public string Occurrence { get; set; }

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