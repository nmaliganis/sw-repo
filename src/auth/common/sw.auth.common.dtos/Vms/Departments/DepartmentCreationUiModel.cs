using sw.infrastructure.DTOs.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.Vms.Departments {
    public class DepartmentCreationUiModel : IUiModel
    {
        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Message { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Notes { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }

        [Required]
        [Editable(true)]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Editable(true)]
        public DateTime ModifiedDate { get; set; }

        [Required]
        [Editable(true)]
        public bool Active { get; set; }
    }
}