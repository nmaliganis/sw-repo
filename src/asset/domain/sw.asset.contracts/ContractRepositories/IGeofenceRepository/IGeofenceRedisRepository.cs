using sw.asset.common.dtos.Vms.Geofence;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sw.asset.contracts.ContractRepositories.IGeofenceRepository;

public interface IGeofenceRedisRepository
{
    Task<GeoPosition?> GetGeoPointAsync(string key, RedisValue pointMember);
    Task<GeoPosition?[]> GetGeoPointsAsync(string key, RedisValue[] pointMember);
    Task<bool> AddGeoPointAsync(string key, GeoEntry point);
    Task<long> AddGeofencePointAsync(string key, List<GeoEntry> points);
    Task<bool> DeleteGeoPointAsync(string key, GeoEntry point);
    Task<bool> DeleteGeoPointAsync(string key);
    Task<long> GetCountOfGeofenceEntries(string key);
    Task<RedisValue[]> GetGeofenceEntries(string key);
    Task<RedisValue> GetPointEntry(string key);

    Task<bool> AddMunicipalityAsync(string key, long municipalityId, string municipalityName);
    Task<bool> AddMapPointsAsync(string key, List<MapUiModel> geoFencePoints);
    //Task<List<GeoEntry>> GetMapPointsAsync(string key);
}// Interface: IGeofenceRedisRepository