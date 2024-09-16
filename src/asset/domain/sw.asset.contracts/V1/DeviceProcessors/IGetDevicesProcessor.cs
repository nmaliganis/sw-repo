using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.DeviceProcessors;

public interface IGetDevicesProcessor
{
  Task<BusinessResult<PagedList<DeviceUiModel>>> GetDevicesAsync(GetDevicesQuery qry);
}