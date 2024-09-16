using System;
using System.ComponentModel.DataAnnotations;

namespace sw.localization.common.dtos.Vms.Bases
{
    public interface IUiModel
    {
        [Key]
        long Id { get; set; }
        [Editable(false)]
        string Message { get; set; }
    }
}
