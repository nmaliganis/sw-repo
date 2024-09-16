using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.ResourceParameters.Users
{
    public class DeleteUserResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
