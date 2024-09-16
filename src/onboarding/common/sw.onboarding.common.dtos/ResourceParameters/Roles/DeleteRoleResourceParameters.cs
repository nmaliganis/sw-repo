using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.ResourceParameters.Roles
{
    public class DeleteRoleResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
