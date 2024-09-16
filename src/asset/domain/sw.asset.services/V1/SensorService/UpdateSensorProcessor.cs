using sw.asset.common.dtos.Cqrs.Sensor;
using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.Sensors;

namespace sw.asset.services.V1.SensorService
{
    public class UpdateSensorProcessor :
        IUpdateSensorProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly ISensorRepository _sensorRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateSensorProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            ISensorRepository sensorRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _sensorRepository = sensorRepository;
        }

        public async Task<BusinessResult<SensorModificationUiModel>> UpdateSensorAsync(UpdateSensorCommand updateCommand)
        {
            var bc = new BusinessResult<SensorModificationUiModel>(new SensorModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var sensor = _sensorRepository.FindBy(updateCommand.Id);
            if (sensor is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Sensor Id does not exist"));
                return bc;
            }

            var modifiedSensor = _autoMapper.Map<Sensor>(updateCommand);
            sensor.Modified(updateCommand.ModifiedById, modifiedSensor);

            Persist(sensor, updateCommand.Id);

            var response = _autoMapper.Map<SensorModificationUiModel>(sensor);
            response.Message = $"Sensor id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Sensor sensor, long id)
        {
            _sensorRepository.Save(sensor, id);
            _uOf.Commit();
        }
    }
}
