using sw.infrastructure.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.Vms.EventPositions
{
    public class EventPositionModificationUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Editable(true)]
        public double Latitude { get; set; }

        [Editable(true)]
        public double Longitude { get; set; }

        [Editable(true)]
        public long GeocodedPositionId { get; set; }
    }
}
