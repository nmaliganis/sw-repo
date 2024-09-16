using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.asset.services.V1.DeviceModelService
{
    public class GetDeviceModelByIdProcessor :
        IGetDeviceModelByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDeviceModelRepository _deviceModelRepository;
        public GetDeviceModelByIdProcessor(IDeviceModelRepository deviceModelRepository, IAutoMapper autoMapper)
        {
            _deviceModelRepository = deviceModelRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<DeviceModelUiModel>> GetDeviceModelByIdAsync(long id)
        {
            var bc = new BusinessResult<DeviceModelUiModel>(new DeviceModelUiModel());

            var deviceModel = _deviceModelRepository.FindActiveById(id);
            if (deviceModel is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "DeviceModel Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<DeviceModelUiModel>(deviceModel);
            response.Message = $"DeviceModel id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
