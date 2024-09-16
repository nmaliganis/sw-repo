using sw.asset.common.dtos.Cqrs.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.VehicleProcessors;
using sw.asset.model.Assets.Vehicles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.VehicleService
{
    public class DeleteSoftVehicleProcessor :
        IDeleteSoftVehicleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IVehicleRepository _vehicleRepository;

        public DeleteSoftVehicleProcessor(IUnitOfWork uOf, IVehicleRepository vehicleRepository)
        {
            _uOf = uOf;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<BusinessResult<VehicleDeletionUiModel>> DeleteSoftVehicleAsync(DeleteSoftVehicleCommand deleteCommand)
        {
            var bc = new BusinessResult<VehicleDeletionUiModel>(new VehicleDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var vehicle = _vehicleRepository.FindBy(deleteCommand.Id);
            if (vehicle is null || !vehicle.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Vehicle Id does not exist"));
                return bc;
            }

            vehicle.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(vehicle, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"Vehicle with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Vehicle vehicle, long id)
        {
            _vehicleRepository.Save(vehicle, id);
            _uOf.Commit();
        }
    }
}
