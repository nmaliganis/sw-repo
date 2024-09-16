using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Assets.Containers;

public class CreateContainerResourceParameters : AssetBaseResourceParameters
{
    [Required]
    public bool IsVisible { get; set; }

    [Required]
    public long Level { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    [Required]
    public double TimeToFull { get; set; }

    [Required]
    public DateTime LastServicedDate { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public DateTime MandatoryPickupDate { get; set; }

    [Required]
    public bool MandatoryPickupActive { get; set; }

    [Required]
    public int Capacity { get; set; }

    [Required]
    public int WasteType { get; set; }

    [Required]
    public int Material { get; set; }

}//Class : CreateContainerResourceParameters