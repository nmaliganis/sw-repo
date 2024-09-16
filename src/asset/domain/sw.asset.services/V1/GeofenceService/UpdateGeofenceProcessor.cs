using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.ContractRepositories.IGeofenceRepository;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.services.V1.GeofenceService
{
    public class UpdateGeofenceProcessor :
        IUpdateGeofenceProcessor
    {
        private readonly IGeofenceRedisRepository _geofenceRepository;
        public UpdateGeofenceProcessor(IGeofenceRedisRepository geofenceRepository)
        {
            _geofenceRepository = geofenceRepository;
        }

        public async Task<BusinessResult<GeofenceModificationUiModel>> UpdateGeofenceAsync(UpdateGeofenceCommand updateCommand)
        {
            //var geofencePoints = new List<GeoEntry>();

            //foreach (var mapPoint in updateCommand.Parameters.GeoFencePoints)
            //{
            //    GeoEntry itemGeoEntryToBeAdded = new GeoEntry(mapPoint.Longitude, mapPoint.Latitude,
            //      GetLocationByCoordinates(mapPoint.Latitude, mapPoint.Longitude).Result);
            //    geofencePoints.Add(itemGeoEntryToBeAdded);
            //}

            //try
            //{
            //    var geoPointCreation = await _geofenceRepository.AddGeofencePointAsync(geofenceId, geofencePoints);
            //    var geoMapPointStored = await _geofenceRepository.AddMapPointsAsync($"{geofenceId}-s",
            //      geofenceForModification.GeoFencePoints);

            //    if (geoMapPointStored)
            //        Ok(new { geoPointCreation, geofencePoints });

            //    return BadRequest("MODIFICATION_GEOFENCE_FAILED");
            //}

            return null;
        }

    }
}
