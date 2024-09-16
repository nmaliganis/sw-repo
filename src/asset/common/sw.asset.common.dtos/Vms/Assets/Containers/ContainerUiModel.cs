using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.Vms.Assets.Containers;

public class ContainerUiModel : AssetBaseUiModel
{
    [Editable(true)]
    public bool IsVisible { get; set; }

    [Editable(true)]
    public long Level { get; set; }
    [Editable(true)]
    public long PrevLevel { get; set; }


    [Editable(true)]
    public double Latitude { get; set; }

    [Editable(true)]
    public double Longitude { get; set; }


    [Editable(true)]
    public double TimeToFull { get; set; }


    [Editable(true)]
    public DateTime LastServicedDate { get; set; }

    [Editable(true)]
    public DateTime MandatoryPickupDate { get; set; }

    [Editable(true)]
    public bool MandatoryPickupActive { get; set; }
    [Editable(true)]
    public DateTime LastUpdated { get; set; }



    [Editable(true)]
    public int Capacity { get; set; }

    [Editable(true)]
    public string CapacityLocale { get; set; }


    [Editable(true)]
    public int WasteType { get; set; }

    [Editable(true)]
    public string WasteTypeLocale { get; set; }


    [Editable(true)]
    public int ContainerStatus { get; set; }

    [Editable(true)]
    public string ContainerStatusLocale { get; set; }

    [Editable(true)]
    public int BinStatus { get; set; }

    [Editable(true)]
    public string BinStatusLocale { get; set; }

    [Editable(true)]
    public int Material { get; set; }

    [Editable(true)]
    public string MaterialLocale { get; set; }

    [Editable(true)]
    public int ZoneId { get; set; }

    [Editable(true)]
    public string ZoneName { get; set; }

}