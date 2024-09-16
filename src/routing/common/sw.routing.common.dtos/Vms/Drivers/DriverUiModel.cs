using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.routing.common.dtos.Vms.Drivers
{
    public class DriverUiModel : IUiModel
    {
        [Editable(true)] public string Firstname { get; set; }
        [Editable(true)] public string Lastname { get; set; }
        [Editable(true)] public string Email { get; set; }
        [Editable(true)] public long Member { get; set; }
        [Editable(true)] public long Id { get; set; }

        public string Message { get; set; }

        [Required] [Editable(true)] public DateTime CreatedDate { get; set; }
        [Required] [Editable(true)] public DateTime ModifiedDate { get; set; }
        [Required] [Editable(true)] public bool Active { get; set; }
    }
}