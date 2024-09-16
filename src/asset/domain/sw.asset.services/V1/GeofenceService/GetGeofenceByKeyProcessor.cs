using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.ContractRepositories.IGeofenceRepository;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sw.asset.services.V1.GeofenceService;

public class GetGeofenceByKeyProcessor :
    IGetGeofenceByKeyProcessor
{
    private readonly IGeofenceRedisRepository _geofenceRepository;
    public GetGeofenceByKeyProcessor(IGeofenceRedisRepository geofenceRepository)
    {
        _geofenceRepository = geofenceRepository;
    }
    public async Task<BusinessResult<LandmarkUiModel>> GetGeofenceByKeyAsync(string key)
    {
        var bc = new BusinessResult<LandmarkUiModel>(new LandmarkUiModel());

        var geofenceStoredPointCount = await _geofenceRepository.GetCountOfGeofenceEntries(key);

        List<List<double>> positions = new List<List<double>>();
        if (geofenceStoredPointCount >= 0)
        {
            var geofenceStoredRedisValues = await _geofenceRepository.GetGeofenceEntries(key);
            var geoStoredPoints = await _geofenceRepository.GetGeoPointsAsync(key, geofenceStoredRedisValues);
            foreach (var geoPoint in geoStoredPoints)
            {
                if (geoPoint != null)
                {
                    positions.Add(new List<double>() { geoPoint.Value.Latitude, geoPoint.Value.Longitude });
                }
            }
        }

        bc.Model.Positions = positions;
        bc.Model.Name = key;
        bc.Model.Message = $"{geofenceStoredPointCount} Points fetched succesfully";

        return await Task.FromResult(bc);
    }
}// Class: GetGeofenceByKeyProcessor
