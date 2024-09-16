using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace sw.asset.common.dtos.Vms.Assets.Containers.Types;

[JsonConverter(typeof(StringEnumConverter))]
public enum MaterialType
{
    HDPE = 1,
    Metallic,
    Other
}//Enum : MaterialType