using System.ComponentModel.DataAnnotations;

namespace sw.admin.common.dtos.V1.ResourceParameters.DepartmentPersonRoles
{
    public class CreateDepartmentPersonRoleResourceParameters
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string CodeErp { get; set; }

        public string Notes { get; set; }

        [Required]
        public long DepartmentId { get; set; }
    }
}
