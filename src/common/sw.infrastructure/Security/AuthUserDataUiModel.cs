using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.infrastructure.Security {
    public class AuthUserDataUiModel {
        [Required]
        [Editable(false)]
        public long MemberId { get; set; }
        [Required]
        [Editable(false)]
        public long CompanyId { get; set; }
        [Required]
        [Editable(false)]
        public List<long> DepartmentIds { get; set; }
    }
}