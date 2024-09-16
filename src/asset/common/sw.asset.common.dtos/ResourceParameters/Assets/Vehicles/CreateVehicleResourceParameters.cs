using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Assets.Vehicles
{
    public class CreateVehicleResourceParameters : AssetBaseResourceParameters
    {
        [Required]
        public string NumPlate { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public DateTime RegisteredDate { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int Gas { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public double Axels { get; set; }

        public double MinTurnRadius { get; set; }

        public double Length { get; set; }
    }
}
