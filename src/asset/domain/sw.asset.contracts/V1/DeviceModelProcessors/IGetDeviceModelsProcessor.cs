using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.DeviceModelProcessors
{
    public interface IGetDeviceModelsProcessor
    {
        Task<BusinessResult<PagedList<DeviceModelUiModel>>> GetDeviceModelsAsync(GetDeviceModelsQuery qry);
    }
}
