using System.ComponentModel.DataAnnotations;

namespace sw.admin.common.dtos.V1.ResourceParameters.Departments
{
    public class DeleteDepartmentResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
