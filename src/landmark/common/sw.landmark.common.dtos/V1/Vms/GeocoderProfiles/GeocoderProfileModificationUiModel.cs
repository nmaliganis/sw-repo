using sw.infrastructure.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.Vms.GeocoderProfiles
{
    public class GeocoderProfileModificationUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string SourceName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Params { get; set; }
    }
}
