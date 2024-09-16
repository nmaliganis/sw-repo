using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.routing.common.dtos.Vms.Vehicles
{
    public class VehicleTransportCombinationUiModel : IUiModel
    {
        [Editable(true)]
        public VehicleUiModel Vehicle { get; set; }
        [Editable(true)]
        public string Type { get; set; }
        [Editable(true)]
        public long Id { get; set; }

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