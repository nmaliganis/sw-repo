using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Events;

public record EventHistoryUiModel 
{
    public string Message { get; set; }

    [Editable(true)]
    public Guid Id { get; set; }

    [Required]
    public bool IsWastePickUp { get; set; }

    [Required]
    public DateTime Recorded { get; set; }

    [Required]
    public DateTime Received { get; set; }

    [Required]
    public double EventValue { get; set; }

    [Required]
    public string EventValueJson { get; set; }
}
