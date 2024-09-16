using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.admin.common.dtos.V1.Vms.Persons
{
    public class PersonModificationUiModel : IUiModel
    {
        public string Message { get; set; }

        [Key]
        [Editable(true)]
        public long Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string LastName { get; set; }

        [Required]
        [Editable(true)]
        public long Gender { get; set; }

        [Editable(true)]
        public string Phone { get; set; }

        [Editable(true)]
        public string ExtPhone { get; set; }

        [Editable(true)]
        public string Notes { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Email { get; set; }

        [Editable(true)]
        public string AddressStreet1 { get; set; }

        [Editable(true)]
        public string AddressStreet2 { get; set; }

        [Editable(true)]
        public string AddressPostCode { get; set; }

        [Editable(true)]
        public string AddressCity { get; set; }

        [Editable(true)]
        public string AddressRegion { get; set; }

        [Editable(true)]
        public string Mobile { get; set; }

        [Editable(true)]
        public string ExtMobile { get; set; }

        [Required]
        [Editable(true)]
        public long Status { get; set; }

        [Required]
        [Editable(true)]
        public long PersonRoleId { get; set; }
    }
}
