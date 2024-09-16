using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.ResourceParameters.Companies;

public class DeleteCompanyResourceParameters
{
  [Required]
  public string DeletedReason { get; set; }
}