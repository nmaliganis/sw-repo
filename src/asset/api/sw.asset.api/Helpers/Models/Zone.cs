using System.Collections.Generic;

namespace sw.asset.api.Helpers.Models;

/// <summary>
/// Class : Zone
/// </summary>
public class Zone
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<List<double>> Positions { get; set; }
}