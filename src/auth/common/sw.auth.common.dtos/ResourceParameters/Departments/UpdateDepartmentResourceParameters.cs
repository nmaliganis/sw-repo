using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.ResourceParameters.Departments
{
    public class UpdateDepartmentResourceParameters
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string CodeErp { get; set; }

        public string Notes { get; set; }
    }
}
