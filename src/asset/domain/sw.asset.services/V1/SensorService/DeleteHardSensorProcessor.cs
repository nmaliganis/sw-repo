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
    public class DeleteHardSensorProcessor :
        IDeleteHardSensorProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ISensorRepository _sensorRepository;

        public DeleteHardSensorProcessor(IUnitOfWork uOf, ISensorRepository sensorRepository)
        {
            _uOf = uOf;
            _sensorRepository = sensorRepository;
        }

        public async Task<BusinessResult<SensorDeletionUiModel>> DeleteHardSensorAsync(DeleteHardSensorCommand deleteCommand)
        {
            var bc = new BusinessResult<SensorDeletionUiModel>(new SensorDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var Sensor = _sensorRepository.FindBy(deleteCommand.Id);
            if (Sensor is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Sensor Id does not exist"));
                return bc;
            }

            Persist(Sensor);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"Sensor with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Sensor Sensor)
        {
            _sensorRepository.Remove(Sensor);
            _uOf.Commit();
        }
    }
}
