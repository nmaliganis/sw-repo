using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.Sensors;

namespace sw.asset.services.V1.SensorService
{
    public class DeleteSoftSensorProcessor :
        IDeleteSoftSensorProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ISensorRepository _sensorRepository;

        public DeleteSoftSensorProcessor(IUnitOfWork uOf, ISensorRepository sensorRepository)
        {
            _uOf = uOf;
            _sensorRepository = sensorRepository;
        }

        public async Task<BusinessResult<SensorDeletionUiModel>> DeleteSoftSensorAsync(DeleteSoftSensorCommand deleteCommand)
        {
            var bc = new BusinessResult<SensorDeletionUiModel>(new SensorDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var sensor = _sensorRepository.FindBy(deleteCommand.Id);
            if (sensor is null || !sensor.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Sensor Id does not exist"));
                return bc;
            }

            sensor.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(sensor, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"Sensor with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Sensor sensor, long id)
        {
            _sensorRepository.Save(sensor, id);
            _uOf.Commit();
        }
    }
}
