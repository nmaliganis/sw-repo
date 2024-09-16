using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.GeofenceProcessors;

public interface IGetGeofencesProcessor
{
  Task<BusinessResult<PagedList<GeofenceUiModel>>> GetGeofencesAsync(GetGeofencesQuery qry);
}// Interface: IGetGeofencesProcessor