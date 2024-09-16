using System;
using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.Vms.Assets.Vehicles
{
    public class VehicleModificationUiModel : AssetBaseUiModel
    {
        [Editable(true)]
        public bool IsVisible { get; set; }

        [Editable(true)]
        public long Level { get; set; }

        [Editable(true)]
        public double Latitude { get; set; }

        [Editable(true)]
        public double Longitude { get; set; }

        [Editable(true)]
        public double TimeToFull { get; set; }

        [Editable(true)]
        public DateTime LastServicedDate { get; set; }

        [Editable(true)]
        public int Status { get; set; }

        [Editable(true)]
        public DateTime MandatoryPickupDate { get; set; }

        [Editable(true)]
        public bool MandatoryPickupActive { get; set; }

        [Editable(true)]
        public int Capacity { get; set; }

        [Editable(true)]
        public int WasteType { get; set; }

        [Editable(true)]
        public int Material { get; set; }
    }
}
