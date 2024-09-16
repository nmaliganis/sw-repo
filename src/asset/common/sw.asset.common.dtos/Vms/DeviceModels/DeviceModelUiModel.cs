using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.DeviceModels
{
    public class DeviceModelUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeName { get; set; }

        [Editable(true)]
        public bool Enabled { get; set; }
    }
}
