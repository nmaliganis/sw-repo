using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sw.routing.common.dtos.Vms.Vehicles;
using sw.infrastructure.DTOs.Base;

namespace sw.routing.common.dtos.Vms.TransportCombination
{
    public class TransportCombinationUiModel : IUiModel
    {
        [Editable(true)] public long Id { get; set; }
        [Editable(true)]
        public List<VehicleTransportCombinationUiModel> Vehicles { get; set; }

        public string Message { get; set; }

        [Required] [Editable(true)] public DateTime CreatedDate { get; set; }
        [Required] [Editable(true)] public DateTime ModifiedDate { get; set; }
        [Required] [Editable(true)] public bool Active { get; set; }
    }
}