using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.asset.model.Assets.Vehicles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.VehicleService
{
    public class UpdateVehicleProcessor :
        IUpdateVehicleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateVehicleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IVehicleRepository vehicleRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<BusinessResult<VehicleModificationUiModel>> UpdateVehicleAsync(UpdateVehicleCommand updateCommand)
        {
            var bc = new BusinessResult<VehicleModificationUiModel>(new VehicleModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var vehicle = _vehicleRepository.FindBy(updateCommand.Id);
            if (vehicle is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Vehicle Id does not exist"));
                return bc;
            }

            var modifiedVehicle = _autoMapper.Map<Vehicle>(updateCommand);
            vehicle.Modified(updateCommand.ModifiedById, modifiedVehicle);

            Persist(vehicle, updateCommand.Id);

            var response = _autoMapper.Map<VehicleModificationUiModel>(vehicle);
            response.Message = $"Vehicle id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Vehicle vehicle, long id)
        {
            _vehicleRepository.Save(vehicle, id);
            _uOf.Commit();
        }
    }
}
