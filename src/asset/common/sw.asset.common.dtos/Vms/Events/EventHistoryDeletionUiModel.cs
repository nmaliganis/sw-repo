using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Events;

public record EventHistoryDeletionUiModel : IUiModel
{
    [Required]
    [Editable(false)]
    public bool Successful { get; set; }

    [Editable(false)]
    public long Id { get; set; }

    public string Message { get; set; }
}
