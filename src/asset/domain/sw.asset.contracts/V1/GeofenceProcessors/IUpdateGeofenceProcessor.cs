using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.GeofenceProcessors;

public interface IUpdateGeofenceProcessor
{
    Task<BusinessResult<GeofenceModificationUiModel>> UpdateGeofenceAsync(UpdateGeofenceCommand updateCommand);
}// Interface: IUpdateGeofenceProcessor
