using sw.asset.common.dtos.Vms.Geofence;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.model.Geofence;

public class UpdateGeofenceResourceParameters
{
    [Required]
    [Editable(true)]
    public List<MapUiModel> GeoFencePoints { get; set; }

}