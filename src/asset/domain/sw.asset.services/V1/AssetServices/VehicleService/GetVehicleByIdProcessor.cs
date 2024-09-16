using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.VehicleService
{
    public class GetVehicleByIdProcessor :
        IGetVehicleByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IVehicleRepository _vehicleRepository;
        public GetVehicleByIdProcessor(IVehicleRepository vehicleRepository, IAutoMapper autoMapper)
        {
            _vehicleRepository = vehicleRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<VehicleUiModel>> GetVehicleByIdAsync(long id)
        {
            var bc = new BusinessResult<VehicleUiModel>(new VehicleUiModel());

            var vehicle = _vehicleRepository.FindActiveBy(id);
            if (vehicle is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Vehicle Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<VehicleUiModel>(vehicle);
            response.Message = $"Vehicle id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
