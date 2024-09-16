using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Companies.Zones;

public class PointUiModel
{
    [Required]
    [Editable(true)]
    public double Lon { get; set; }

    [Required]
    [Editable(true)]
    public double Lat { get; set; }
}