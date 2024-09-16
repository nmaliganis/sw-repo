using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.SensorTypes
{
    public record SensorTypeUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Editable(true)]
        public bool ShowAtStatus { get; set; }

        [Editable(true)]
        public long StatusExpiryMinutes { get; set; }

        [Editable(true)]
        public bool ShowOnMap { get; set; }

        [Editable(true)]
        public bool ShowAtReport { get; set; }

        [Editable(true)]
        public bool ShowAtChart { get; set; }

        [Editable(true)]
        public bool ResetValues { get; set; }

        [Editable(true)]
        public bool SumValues { get; set; }

        [Editable(true)]
        public long Precision { get; set; }

        [Editable(true)]
        public string Tunit { get; set; }

        [Editable(true)]
        public bool CalcPosition { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }
        [Required]
        public int SensorTypeIndex { get; set; }
    }
}
