using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.DeviceModels
{
    public class DeviceModelCreationUiModel : IUiModel
    {
        [Key]
        [Editable(true)]
        public long Id { get; set; }

        public string Message { get; set; }
    }
}
