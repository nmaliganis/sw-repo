using sw.asset.common.dtos.Cqrs.Geofences;
using sw.asset.common.dtos.Vms.Geofence;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.GeofenceProcessors;

public interface ICreateGeofenceProcessor
{
    Task<BusinessResult<GeofenceUiModel>> CreateGeofenceAsync(CreateGeofenceCommand createCommand);
    Task<BusinessResult<GeoEntryUiModel>> CreateGeoEntryAsync(CreateGeoEntryCommand createCommand);
    Task<BusinessResult<MunicipalityUiModel>> CreateMunicipalityAsync(CreateMunicipalityCommand createCommand);

}// Interface: ICreateGeofenceProcessor