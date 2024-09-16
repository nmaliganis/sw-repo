namespace sw.asset.common.dtos.ResourceParameters.Assets.Containers;

public class UpdateContainerWithMeasurementsResourceParameters
{
  public double Range { get; set; }
  public double Status { get; set; }
  public double Temperature { get; set; }
  public double Latitude { get; set; }
  public double Longitude { get; set; }
  public double Altitude { get; set; }
  public double Speed { get; set; }
  public double Direction { get; set; }
  public int FixMode { get; set; }
  public double Hdop { get; set; }
  public int SatellitesUsed { get; set; }
}