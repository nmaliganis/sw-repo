using System;
using System.ComponentModel.DataAnnotations;
using sw.routing.common.dtos.Vms.Drivers;
using sw.routing.common.dtos.Vms.TransportCombination;
using sw.infrastructure.DTOs.Base;

namespace sw.routing.common.dtos.Vms.DriversTransportCombinations
{
    public class DriverTransportCombinationUiModel : IUiModel
    {
        [Editable(true)]
        public DriverUiModel Driver { get; set; }
        [Editable(true)]
        public TransportCombinationUiModel TransportCombination { get; set; }
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