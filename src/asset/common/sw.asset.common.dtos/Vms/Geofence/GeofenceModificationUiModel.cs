using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Geofence
{
    public class GeofenceModificationUiModel : IUiModel
    {
        public long Id { get; set; }

        public string Message { get; set; }
    }
}
