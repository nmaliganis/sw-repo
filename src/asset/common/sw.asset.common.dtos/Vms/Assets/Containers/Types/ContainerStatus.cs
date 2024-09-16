using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;

namespace sw.asset.common.dtos.Vms.Assets.Containers.Types;

[JsonConverter(typeof(StringEnumConverter))]
public enum ContainerStatus
{
    Normal = 1,
    BrokenLid = 2,
    BrokenBucket = 3,
    DamagedPedal = 4,
    DamagedWheel = 5,
    Burned = 6,
    ChangedPosition = 7,
    Missing = 8

}// Class: ContainerStatus