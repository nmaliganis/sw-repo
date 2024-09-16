using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetProcessors.VehicleProcessors
{
    public interface IGetVehicleByIdProcessor
    {
        Task<BusinessResult<VehicleUiModel>> GetVehicleByIdAsync(long id);
    }
}
