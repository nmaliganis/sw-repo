using sw.asset.common.dtos.Vms.Geofence;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Geofences;

public class CreateGeofenceResourceParameters
{
    [Required]
    [Editable(false)]
    public List<GeoEntryUiModel> GeoPoints { get; set; }
    [Required]
    [Editable(false)]
    public string PointId { get; set; }
}// Class: CreateGeofenceResourceParameters