using System;
using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.auth.common.dtos.Vms.Companies
{
    public class CompanyUiModel : IUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string CodeErp { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Description { get; set; }
        [Editable(true)]
        public long Id { get; set; }

        public string Message { get; set; }

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