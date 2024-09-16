using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.asset.services.V1.DeviceService
{
    public class GetDeviceByIdProcessor :
        IGetDeviceByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDeviceRepository _deviceRepository;
        public GetDeviceByIdProcessor(IDeviceRepository deviceRepository, IAutoMapper autoMapper)
        {
            _deviceRepository = deviceRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<DeviceUiModel>> GetDeviceByIdAsync(long id)
        {
            var bc = new BusinessResult<DeviceUiModel>(new DeviceUiModel());

            var device = _deviceRepository.FindActiveById(id);
            if (device is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Device Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<DeviceUiModel>(device);
            response.Message = $"Device id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
