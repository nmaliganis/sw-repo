using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.landmark.common.dtos.V1.Vms.EventHistories;

public class EventHistoryUiModel : IUiModel
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