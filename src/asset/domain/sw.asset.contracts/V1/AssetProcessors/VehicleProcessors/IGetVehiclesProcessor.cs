using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetProcessors.VehicleProcessors
{
    public interface IGetVehiclesProcessor
    {
        Task<BusinessResult<PagedList<VehicleUiModel>>> GetVehiclesAsync(GetVehiclesQuery qry);
    }
}
