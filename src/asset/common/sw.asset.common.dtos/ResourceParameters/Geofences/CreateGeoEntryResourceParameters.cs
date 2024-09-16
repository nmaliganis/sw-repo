using sw.asset.common.dtos.Vms.Geofence;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Geofences;

public class CreateGeoEntryResourceParameters
{
    [Required]
    [Editable(false)]
    public GeoPointUiModel GeoPoint { get; set; }
    [Required]
    [Editable(false)]
    public string PointId { get; set; }

}// Class: CreateGeoEntryResourceParameters
