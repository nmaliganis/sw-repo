using sw.infrastructure.DTOs.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.Vms.EventHistories
{
    public class EventHistoryModificationUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Editable(true)]
        public long SensorId { get; set; }

        [Editable(true)]
        public long EventPositionId { get; set; }

        [Editable(true)]
        public DateTimeOffset Recorded { get; set; }

        [Editable(true)]
        public DateTimeOffset Received { get; set; }

        [Editable(true)]
        public double EventValue { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string EventValueJson { get; set; }
    }
}
