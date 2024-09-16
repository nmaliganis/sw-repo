using sw.infrastructure.DTOs.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.Vms.Geofence;

public class GeofenceUiModel : IUiModel
{
    public long Id { get; set; }

    public string Message { get; set; }
    [Required]
    [Editable(false)]
    public List<GeoEntryUiModel> GeoPoints { get; set; }
    [Required]
    [Editable(false)]
    public string PointId { get; set; }
}// Class: GeofenceUiModel
