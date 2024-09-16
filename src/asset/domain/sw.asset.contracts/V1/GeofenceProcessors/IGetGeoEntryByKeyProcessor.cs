using sw.asset.common.dtos.Vms.Geofence;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.GeofenceProcessors;

public interface IGetGeoEntryByKeyProcessor
{
    Task<BusinessResult<GeoEntryUiModel>> GetGeoEntryByKeyAsync(string key);
}// Interface: IGetGeoEntryByKeyProcessor
