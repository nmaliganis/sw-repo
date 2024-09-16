using System.ComponentModel.DataAnnotations;

namespace sw.admin.common.dtos.V1.ResourceParameters.DepartmentPersonRoles
{
    public class DeleteDepartmentPersonRoleResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
