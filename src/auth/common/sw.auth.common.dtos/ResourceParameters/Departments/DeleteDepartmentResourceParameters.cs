using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.ResourceParameters.Departments
{
    public class DeleteDepartmentResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
