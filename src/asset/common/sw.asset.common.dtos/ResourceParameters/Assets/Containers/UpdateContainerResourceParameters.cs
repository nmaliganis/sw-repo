using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Assets.Containers
{
  public class UpdateContainerResourceParameters : AssetBaseResourceParameters
    {
        [Required]
        public long ZoneId { get; set; }
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

        public int Capacity { get; set; }

        public int WasteType { get; set; }

        public int Material { get; set; }
    }
}
