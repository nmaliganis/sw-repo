using System.ComponentModel.DataAnnotations;

namespace sw.admin.common.dtos.V1.ResourceParameters.Persons
{
    public class CreatePersonResourceParameters
    {
        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250)]
        public string LastName { get; set; }

        [Required]
        public long Gender { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(5)]
        public string ExtPhone { get; set; }

        public string Notes { get; set; }

        [Required]
        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(128)]
        public string AddressStreet1 { get; set; }

        [StringLength(128)]
        public string AddressStreet2 { get; set; }

        [StringLength(8)]
        public string AddressPostCode { get; set; }

        [StringLength(64)]
        public string AddressCity { get; set; }

        [StringLength(64)]
        public string AddressRegion { get; set; }

        [StringLength(10)]
        public string Mobile { get; set; }

        [StringLength(5)]
        public string ExtMobile { get; set; }

        [Required]
        public long Status { get; set; }

        [Required]
        public long PersonRoleId { get; set; }
    }
}
