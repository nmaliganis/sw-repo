using sw.infrastructure.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sw.asset.common.dtos.Vms.Geofence;

public class MunicipalityUiModel : IUiModel
{
    public long Id { get; set; }
    public string Message { get; set; }
    public string Name { get; set; }

}
