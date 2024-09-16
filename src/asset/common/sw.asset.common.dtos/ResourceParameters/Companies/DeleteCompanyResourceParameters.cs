using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Companies;

public class DeleteCompanyResourceParameters
{
  [Required]
  public string DeletedReason { get; set; }
}