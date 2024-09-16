using System;
using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Members {
    public class MemberCreationUiModel {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Message { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Description { get; set; }
        [Required]
        [Editable(true)]
        public DateTime CreatedDate { get; set; }
        [Required]
        [Editable(true)]
        public DateTime ModifiedDate { get; set; }
        [Required]
        [Editable(true)]
        public bool Active { get; set; }
        [Editable(true)]
        public long Id { get; set; }
    }
}