using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.SensorTypes
{
    public record SensorTypeDeletionUiModel : IUiModel
    {
        [Required]
        [Editable(false)]
        public bool Successful { get; set; }

        [Editable(false)]
        public long Id { get; set; }

        [Editable(false)]
        public bool Hard { get; set; }

        public string Message { get; set; }
    }
}
