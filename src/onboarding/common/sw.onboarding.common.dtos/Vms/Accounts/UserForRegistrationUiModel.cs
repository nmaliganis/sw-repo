using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.Vms.Accounts
{
    public class UserForRegistrationUiModel
    {
        //UserModel
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Login { get; set; }
        [MinLength(8)]
        [MaxLength(16)]
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Password { get; set; }
        
        
        //MemberModel
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Firstname { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Lastname { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Email { get; set; }
        [Required]
        [Editable(true)]
        public int Gender { get; set; }
        [Required]
        [Editable(true)]
        public string GenderValue { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Phone { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ExtPhone { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Mobile { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ExtMobile { get; set; }
        [Editable(true)]
        public string Notes { get; set; }

        //AddressModel
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Street { get; set; }
        [Editable(true)]
        public string StreetNumber { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string AddressPostCode { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string AddressCity { get; set; }
        [Editable(true)]
        public string AddressRegion { get; set; }
        
        
        //Dependencies Model
        [Required]
        [Editable(true)]
        public List<long> RoleIds { get; set; }

        [Required]
        [Editable(true)]
        public List<long> DepartmentIds { get; set; }

        [Required]
        [Editable(true)]
        public long Company { get; set; }
    }
}