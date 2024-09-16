using System.ComponentModel.DataAnnotations;

namespace sw.admin.common.dtos.V1.ResourceParameters.Persons
{
    public class DeletePersonResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
