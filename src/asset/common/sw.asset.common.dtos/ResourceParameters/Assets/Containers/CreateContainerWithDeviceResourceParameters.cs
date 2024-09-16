using System;
using System.ComponentModel.DataAnnotations;
using sw.asset.common.dtos.Vms.Assets.Containers.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace sw.asset.common.dtos.ResourceParameters.Assets.Containers;

public class CreateContainerWithDeviceResourceParameters : AssetBaseResourceParameters
{
    public long Level { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime MandatoryPickupDate { get; set; }

    public bool MandatoryPickupActive { get; set; }

    public int Capacity { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ContainerStatus Status { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public ContainerType ContainerType { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public MaterialType Material { get; set; }

}// Class: CreateContainerWithDeviceResourceParameters