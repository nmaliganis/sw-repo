using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.DeviceProcessors;

public interface ICreateDeviceProcessor
{
  Task<BusinessResult<DeviceUiModel>> CreateDeviceAsync(CreateDeviceCommand createCommand);
}