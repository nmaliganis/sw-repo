using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.ContractRepositories.IGeofenceRepository;
using sw.asset.model.Geofence;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.repository.Repositories.Geofence;

public class GeofenceRedisRepository : IGeofenceRedisRepository
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public GeofenceRedisRepository(ConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = redis.GetDatabase();
    }

    private string Serialize(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
    private T Deserialize<T>(string serialized)
    {
        return JsonConvert.DeserializeObject<T>(serialized);
    }

    public async Task<GeoPosition?> GetGeoPointAsync(string key, RedisValue pointMember)
    {
        return await _database.GeoPositionAsync(key, pointMember);
    }
    public async Task<GeoPosition?[]> GetGeoPointsAsync(string key, RedisValue[] pointMembers)
    {
        return await _database.GeoPositionAsync(key, pointMembers);
    }
    public async Task<bool> AddGeoPointAsync(string key, GeoEntry point)
    {
        return await _database.GeoAddAsync(key, point);
    }

    public async Task<bool> AddMunicipalityAsync(string key, long municipalityId, string municipalityName)
    {
        var indexOfMunicipalities = await _database.StringGetAsync(key);
        if (!indexOfMunicipalities.IsNullOrEmpty)
        {
            var municipalitiesStored = await _database.StringGetAsync(key);

            var municipalities = Deserialize<List<Municipality>>(municipalitiesStored);

            if (municipalities == null)
                return false;

            if (municipalities.Any(x => x.Name == municipalityName))
            {
                return false;
            }

            municipalities.Add(new Municipality() { Id = municipalityId, Name = municipalityName });

            return await _database.StringSetAsync(key, Serialize(municipalities));
        }

        return false;
    }

    public async Task<bool> AddMapPointsAsync(string key, List<MapUiModel> geoPoints)
    {
        var indexOfMapPoints = await _database.StringGetAsync(key);
        if (indexOfMapPoints.IsNullOrEmpty)
        {
            return await _database.StringSetAsync(key, Serialize(geoPoints));
        }

        return false;
    }

    //public async Task<List<GeoEntry>> GetMapPointsAsync(string key)
    //{
    //    var mapPointsForGeofence = await _database.SortedSetRangeByRankAsync(key);
    //    return !mapPointsForGeofence.IsNullOrEmpty ? Deserialize<List<GeoEntry>>(mapPointsForGeofence) : null;
    //}
    public async Task<long> AddGeofencePointAsync(string key, List<GeoEntry> points)
    {
        return await _database.GeoAddAsync(key, points.ToArray());
    }
    public async Task<bool> DeleteGeoPointAsync(string key, GeoEntry point)
    {
        return await _database.GeoRemoveAsync(key, point.Member);
    }
    public async Task<bool> DeleteGeoPointAsync(string key)
    {
        return await _database.KeyDeleteAsync(key, CommandFlags.FireAndForget);
    }

    //Sorted Sets
    public async Task<long> GetCountOfGeofenceEntries(string key)
    {
        return await _database.SortedSetLengthAsync(key);
    }
    public async Task<RedisValue[]> GetGeofenceEntries(string key)
    {
        return await _database.SortedSetRangeByRankAsync(key);
    }
    public async Task<RedisValue> GetPointEntry(string key)
    {
        return await Task.Run(() => _database.SortedSetRangeByRank(key).FirstOrDefault());
    }
}// Class: GeofenceRedisRepository