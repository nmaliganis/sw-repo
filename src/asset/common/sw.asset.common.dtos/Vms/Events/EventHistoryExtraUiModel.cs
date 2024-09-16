using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Events;

public record EventHistoryExtraUiModel 
{
    public string Message { get; set; }

    [Editable(true)]
    public Guid Id { get; set; }

    [Required]
    public DateTime Recorded { get; set; }

    [Required]
    public DateTime Received { get; set; }

    [Required]
    public double EventValue { get; set; }

    [Required]
    public string EventValueJson { get; set; }
    [Required]
    public long AssetId { get; set; }
    [Required]
    public long SensorId { get; set; }
    [Required]
    public long DeviceId { get; set; }
}
