using sw.asset.common.dtos.Vms.DeviceModels;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.DeviceModelProcessors
{
    public interface IGetDeviceModelByIdProcessor
    {
        Task<BusinessResult<DeviceModelUiModel>> GetDeviceModelByIdAsync(long id);
    }
}
