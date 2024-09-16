using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace sw.asset.common.dtos.Vms.Assets.Containers.Types;

[JsonConverter(typeof(StringEnumConverter))]
public enum ContainerCapacityType
{
    Eighty_80 = 1,
    ΟneΗundredΑndΤwenty_120 = 2,
    TwoHundredAndForty_240 = 3,
    SixHundredAndSixty_660 = 4,
    OneThousandAndHundred_1100 = 5

}//Enum : ContainerCapacityType