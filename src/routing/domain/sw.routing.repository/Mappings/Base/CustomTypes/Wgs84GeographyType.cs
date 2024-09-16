using System;
using GeoAPI.Geometries;
using NHibernate.Spatial.Type;

namespace sw.routing.repository.Mappings.Base.CustomTypes;

[Serializable]
public class Wgs84GeographyType : PostGisGeometryType
{
    protected override void SetDefaultSRID(IGeometry geometry)
    {
        geometry.SRID = 4326;
    }
}