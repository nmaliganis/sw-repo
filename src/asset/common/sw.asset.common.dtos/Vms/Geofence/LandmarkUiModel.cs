using sw.infrastructure.DTOs.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sw.asset.common.dtos.Vms.Geofence;

public class LandmarkUiModel : IUiModel
{
    public long Id { get; set; }
    public string Message { get; set; }
    [Required]
    [Editable(false)]
    public string Name { get; set; }
    [Required]
    [Editable(false)]
    public List<List<double>> Positions { get; set; }
}
