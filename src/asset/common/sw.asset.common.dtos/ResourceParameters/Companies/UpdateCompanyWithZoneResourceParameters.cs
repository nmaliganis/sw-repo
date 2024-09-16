using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Companies;

public class UpdateCompanyWithZoneResourceParameters
{
    [Required]
    public List<string> Zones { get; set; }
}