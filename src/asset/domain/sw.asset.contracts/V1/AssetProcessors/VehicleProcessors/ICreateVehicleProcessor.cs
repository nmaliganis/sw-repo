using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetProcessors.VehicleProcessors
{
    public interface ICreateVehicleProcessor
    {
        Task<BusinessResult<VehicleCreationUiModel>> CreateVehicleAsync(CreateVehicleCommand createCommand);
    }
}
