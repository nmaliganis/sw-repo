using sw.infrastructure.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.Vms.Geofence;

public class GeoEntryUiModel : IUiModel
{
    public long Id { get; set; }
    public string Message { get; set; }
    [Required]
    [Editable(false)]
    public double Lat { get; set; }
    [Required]
    [Editable(false)]
    public double Lon { get; set; }
    [Required]
    [Editable(false)]
    public string Address { get; set; }

}// Class: GeoEntryUiModel