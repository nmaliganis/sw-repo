using System;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.ResourseParameters.EventHistories
{
    public class CreateEventHistoryResourceParameters
    {
        [Required]
        public long SensorId { get; set; }

        [Required]
        public DateTimeOffset Recorded { get; set; }

        [Required]
        public DateTimeOffset Received { get; set; }

        public double EventValue { get; set; }

        [Required]
        public string EventValueJson { get; set; }

        [Required]
        public long EventPositionId { get; set; }
    }
}
