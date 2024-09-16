using sw.infrastructure.DTOs.Base;
using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.Vms.Departments {
    public class DepartmentModificationUiModel : IUiModel {
        public string Message { get; set; }

        [Key]
        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ModifiedName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ModifiedNotes { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ModifiedCodeErp { get; set; }
    }
}