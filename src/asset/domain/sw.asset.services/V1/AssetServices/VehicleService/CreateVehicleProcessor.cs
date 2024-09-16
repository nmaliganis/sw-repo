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
    public class CreateVehicleProcessor :
        ICreateVehicleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateVehicleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IVehicleRepository vehicleRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<BusinessResult<VehicleCreationUiModel>> CreateVehicleAsync(CreateVehicleCommand createCommand)
        {
            var bc = new BusinessResult<VehicleCreationUiModel>(new VehicleCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _containerRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "Company name already exists"));
            //}

            var vehicle = _autoMapper.Map<Vehicle>(createCommand);
            vehicle.Created(createCommand.CreatedById);

            Persist(vehicle);

            var response = _autoMapper.Map<VehicleCreationUiModel>(vehicle);
            response.Message = "Vehicle created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Vehicle vehicle)
        {
            _vehicleRepository.Add(vehicle);
            _uOf.Commit();
        }
    }
}
