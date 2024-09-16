using System.ComponentModel.DataAnnotations;

namespace sw.asset.common.dtos.ResourceParameters.Assets.Containers;

public class UpdateContainerWithLatLonResourceParameters
{
  [Required]
  public double Lat { get; set; }

  [Required]
  public double Lon { get; set; }
}