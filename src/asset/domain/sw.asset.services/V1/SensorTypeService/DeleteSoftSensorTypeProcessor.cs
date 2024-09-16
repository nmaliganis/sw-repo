using sw.asset.common.dtos.Cqrs.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.SensorTypes;

namespace sw.asset.services.V1.SensorTypeService
{
    public class DeleteSoftSensorTypeProcessor :
        IDeleteSoftSensorTypeProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ISensorTypeRepository _sensorTypeRepository;

        public DeleteSoftSensorTypeProcessor(IUnitOfWork uOf, ISensorTypeRepository sensorTypeRepository)
        {
            _uOf = uOf;
            _sensorTypeRepository = sensorTypeRepository;
        }

        public async Task<BusinessResult<SensorTypeDeletionUiModel>> DeleteSoftSensorTypeAsync(DeleteSoftSensorTypeCommand deleteCommand)
        {
            var bc = new BusinessResult<SensorTypeDeletionUiModel>(new SensorTypeDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var sensorType = _sensorTypeRepository.FindBy(deleteCommand.Id);
            if (sensorType is null || !sensorType.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "SensorType Id does not exist"));
                return bc;
            }

            sensorType.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(sensorType, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"SensorType with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(SensorType sensorType, long id)
        {
            _sensorTypeRepository.Save(sensorType, id);
            _uOf.Commit();
        }
    }
}
