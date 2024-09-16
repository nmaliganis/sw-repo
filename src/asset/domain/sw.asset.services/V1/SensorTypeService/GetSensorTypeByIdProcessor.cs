using sw.asset.common.dtos.Vms.SensorTypes;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SensorTypeProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.asset.services.V1.SensorTypeService
{
    public class GetSensorTypeByIdProcessor :
        IGetSensorTypeByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly ISensorTypeRepository _sensorTypeRepository;
        public GetSensorTypeByIdProcessor(ISensorTypeRepository sensorTypeRepository, IAutoMapper autoMapper)
        {
            _sensorTypeRepository = sensorTypeRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<SensorTypeUiModel>> GetSensorTypeByIdAsync(long id)
        {
            var bc = new BusinessResult<SensorTypeUiModel>(new SensorTypeUiModel());

            var sensorType = _sensorTypeRepository.FindActiveById(id);
            if (sensorType is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "SensorType Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<SensorTypeUiModel>(sensorType);
            response.Message = $"SensorType id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
