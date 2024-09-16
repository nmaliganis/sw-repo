using sw.asset.model.Companies.Zones;
using sw.asset.model.Sensors;
using sw.infrastructure.Extensions;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.model.Assets.Containers.Projections;

public class ContainerProjection
{
    public bool IsVisible { get; set; }
    public long Level { get; set; }
    public long PrevLevel { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double TimeToFull { get; set; }
    public DateTime LastServicedDate { get; set; }
    public int ContainerStatus { get; set; }
    public string ContainerStatusLocale { get; set; }
    public int BinStatus { get; set; }
    public string BinStatusLocale { get; set; }
    public DateTime MandatoryPickupDate { get; set; }
    public bool MandatoryPickupActive { get; set; }
    public DateTime LastUpdated { get; set; }
    public int Capacity { get; set; }
    public int WasteType { get; set; }
    public string WasteTypeLocale { get; set; }
    public int Material { get; set; }
    public string MaterialLocale { get; set; }

}// Class: ContainerProjection