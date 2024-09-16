using System.ComponentModel.DataAnnotations;

namespace sw.admin.common.dtos.V1.ResourceParameters.Companies
{
    public class DeleteCompanyResourceParameters
    {
        [Required]
        public string DeletedReason { get; set; }
    }
}
