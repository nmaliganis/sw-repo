using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using sw.asset.common.dtos.Vms.Assets.Containers.Types;

namespace sw.asset.common.dtos.Vms.Assets.Containers;

public class ContainerCountUiModel
{
    [Editable(true)]
    public int TotalCount { get; set; }

    [Editable(true)]
    public Dictionary<ContainerType, int> Counts { get; set; }
}