using sw.asset.common.dtos.Vms.Geofence;
using sw.asset.model.Geofence;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.GeofenceProcessors;

public interface IGetGeofenceByKeyProcessor
{
    Task<BusinessResult<LandmarkUiModel>> GetGeofenceByKeyAsync(string key);
}// Interface: IGetGeofenceByIdProcessor
