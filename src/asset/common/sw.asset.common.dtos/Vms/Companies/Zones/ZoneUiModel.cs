using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Companies.Zones;

public class ZoneUiModel : IUiModel
{
    public string Message { get; set; }

    [Editable(true)]
    public long Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [Editable(true)]
    public string Name { get; set; }

    [Editable(true)]
    public string Description { get; set; }

    [Required]
    [Editable(true)]
    public List<PointUiModel> Polygon { get; set; }
}