using sw.asset.common.dtos.Vms.Sensors;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.asset.services.V1.SensorService
{
    public class GetSensorByIdProcessor :
        IGetSensorByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ISensorRepository _sensorRepository;
        public GetSensorByIdProcessor(ISensorRepository sensorRepository, IAutoMapper autoMapper)
        {
            _sensorRepository = sensorRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<SensorUiModel>> GetSensorByIdAsync(long id)
        {
            var bc = new BusinessResult<SensorUiModel>(new SensorUiModel());

            var sensor = _sensorRepository.FindActiveById(id);
            if (sensor is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Sensor Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<SensorUiModel>(sensor);
            response.Message = $"Sensor id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
