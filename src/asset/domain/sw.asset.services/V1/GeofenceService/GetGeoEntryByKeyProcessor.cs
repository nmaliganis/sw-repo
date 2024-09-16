using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.ContractRepositories.IGeofenceRepository;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace sw.asset.services.V1.GeofenceService;

public class GetGeoEntryByKeyProcessor :
    IGetGeoEntryByKeyProcessor
{
    private readonly IGeofenceRedisRepository _geofenceRepository;
    public GetGeoEntryByKeyProcessor(IGeofenceRedisRepository geofenceRepository)
    {
        _geofenceRepository = geofenceRepository;
    }

    public async Task<BusinessResult<GeoEntryUiModel>> GetGeoEntryByKeyAsync(string key)
    {
        var bc = new BusinessResult<GeoEntryUiModel>(new GeoEntryUiModel());

        var geoStoredPoint = await _geofenceRepository.GetPointEntry(key);
        GeoPosition? geoPosition;
        if (geoStoredPoint.IsNullOrEmpty)
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_COULD_NOT_FETCH_POINT_BY_KEY"));
            return await Task.FromResult(bc);
        }
        geoPosition = await _geofenceRepository.GetGeoPointAsync(key, geoStoredPoint);
        if (geoPosition == null)
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_COULD_NOT_FETCH_POINT_COORDINATES"));
            return await Task.FromResult(bc);
        }

        bc.Model.Address = key;
        bc.Model.Lat = geoPosition.Value.Latitude;
        bc.Model.Lon = geoPosition.Value.Longitude;


        bc.Model.Message = $"Point fetched succesfully";

        return await Task.FromResult(bc);
    }

}// Class: GetGeofenceByKeyProcessor
