using sw.asset.model.Geofence.Geo;
using System.Collections.Generic;

namespace sw.asset.model.Geofence;

public class Geofence
{
    public string Key { get; set; }
    public List<GeoEntry> GeoPoints { get; set; }
    public string PointId { get; set; }
}// Class: Geofence
