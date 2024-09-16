using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sw.asset.common.dtos.Vms.Companies.Zones;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Companies;

public class CompanyModificationUiModel : IUiModel
{
    [Key]
    [Editable(true)]
    public long Id { get; set; }

    [Key]
    [Editable(true)]
    public List<ZoneUiModel> Zones { get; set; }

    public string Message { get; set; }
}