using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.contracts.ContractRepositories.IGeofenceRepository;
using sw.asset.contracts.V1.GeofenceProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.services.V1.GeofenceService;

public class GetGeofencesProcessor :
  IGetGeofencesProcessor
{
    private readonly IGeofenceRedisRepository _geofenceRepository;

    public GetGeofencesProcessor(IGeofenceRedisRepository geofenceRepository)
    {
        _geofenceRepository = geofenceRepository;
    }

    public async Task<BusinessResult<PagedList<GeofenceUiModel>>> GetGeofencesAsync(GetGeofencesQuery qry)
    {
        return null;
    }
}// Class: GetGeofenceProcessor