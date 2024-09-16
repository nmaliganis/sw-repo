using sw.infrastructure.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sw.asset.common.dtos.Vms.Geofence;

public class MapUiModel : IUiModel
{
    public long Id { get; set; }
    public string Message { get; set; }
    [Required]
    [Editable(true)]
    public double Latitude { get; set; }
    [Required]
    [Editable(true)]
    public double Longitude { get; set; }
    public string Name { get; set; }

}// Class: MapUiModel
