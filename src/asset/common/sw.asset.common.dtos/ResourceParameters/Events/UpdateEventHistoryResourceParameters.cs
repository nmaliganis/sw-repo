using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Events
{
    public record UpdateEventHistoryResourceParameters
    {
        [Required]
        public DateTime Recorded { get; set; }

        [Required]
        public DateTime Received { get; set; }

        [Required]
        public double EventValue { get; set; }

        [Required]
        public string EventValueJson { get; set; }
    }
}
