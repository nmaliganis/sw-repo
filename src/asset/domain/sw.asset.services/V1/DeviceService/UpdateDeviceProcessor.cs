using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.Devices;

namespace sw.asset.services.V1.DeviceService
{
    public class UpdateDeviceProcessor :
        IUpdateDeviceProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateDeviceProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IDeviceRepository deviceRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _deviceRepository = deviceRepository;
        }

        public async Task<BusinessResult<DeviceModificationUiModel>> UpdateDeviceAsync(UpdateDeviceCommand updateCommand)
        {
            var bc = new BusinessResult<DeviceModificationUiModel>(new DeviceModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var device = _deviceRepository.FindBy(updateCommand.Id);
            if (device is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Device Id does not exist"));
                return bc;
            }

            var modifiedDevice = _autoMapper.Map<Device>(updateCommand);
            device.Modified(updateCommand.ModifiedById, modifiedDevice);

            Persist(device, updateCommand.Id);

            var response = _autoMapper.Map<DeviceModificationUiModel>(device);
            response.Message = $"Device id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Device device, long id)
        {
            _deviceRepository.Save(device, id);
            _uOf.Commit();
        }
    }
}
