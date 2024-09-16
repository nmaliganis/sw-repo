using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.Vms.Assets.Vehicles
{
    public class VehicleCreationUiModel : AssetBaseUiModel
    {
        [Editable(true)]
        public string NumPlate { get; set; }

        [Editable(true)]
        public string Brand { get; set; }

        [Editable(true)]
        public DateTime RegisteredDate { get; set; }

        [Editable(true)]
        public int Type { get; set; }

        [Editable(true)]
        public int Status { get; set; }

        [Editable(true)]
        public int Gas { get; set; }

        [Editable(true)]
        public double Height { get; set; }

        [Editable(true)]
        public double Width { get; set; }

        [Editable(true)]
        public double Axels { get; set; }

        [Editable(true)]
        public double MinTurnRadius { get; set; }

        [Editable(true)]
        public double Length { get; set; }
    }
}
