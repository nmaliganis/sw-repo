using GeoAPI.Geometries;
using NHibernate.Spatial.Type;
using System;
using NetTopologySuite.Geometries;

namespace sw.asset.repository.Mappings.Base.CustomeTypes;

[Serializable]
public class Wgs84GeographyType : PostGisGeometryType
{
    protected override void SetDefaultSRID(Geometry geometry)
    {
        geometry.SRID = 4326;
    }
}