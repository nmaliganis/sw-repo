using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace sw.asset.common.dtos.Vms.Assets.Containers.Types;

[JsonConverter(typeof(StringEnumConverter))]
public enum ContainerType
{
    Trash = 1,
    Recycle,
    Compost,
    Other
}

// Class: ContainerCapacity