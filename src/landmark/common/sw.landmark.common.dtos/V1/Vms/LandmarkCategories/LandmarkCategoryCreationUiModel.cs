using sw.infrastructure.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace sw.landmark.common.dtos.V1.Vms.LandmarkCategories
{
    public class LandmarkCategoryCreationUiModel : IUiModel
    {
        public string Message { get; set; }

        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Editable(true)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }

        [Editable(true)]
        public string Params { get; set; }
    }
}
