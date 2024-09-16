using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.asset.model.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.asset.services.V1.DeviceModelService
{
    public class UpdateDeviceModelProcessor :
        IUpdateDeviceModelProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDeviceModelRepository _deviceModelRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateDeviceModelProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IDeviceModelRepository deviceModelRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<BusinessResult<DeviceModelModificationUiModel>> UpdateDeviceModelAsync(UpdateDeviceModelCommand updateCommand)
        {
            var bc = new BusinessResult<DeviceModelModificationUiModel>(new DeviceModelModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var deviceModel = _deviceModelRepository.FindBy(updateCommand.Id);
            if (deviceModel is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "DeviceModel Id does not exist"));
                return bc;
            }

            deviceModel.Modified(
                updateCommand.ModifiedById,
                updateCommand.Name,
                updateCommand.CodeErp,
                updateCommand.CodeName,
                updateCommand.Enabled);

            Persist(deviceModel, updateCommand.Id);

            var response = _autoMapper.Map<DeviceModelModificationUiModel>(deviceModel);
            response.Message = $"DeviceModel id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(DeviceModel deviceModel, long id)
        {
            _deviceModelRepository.Save(deviceModel, id);
            _uOf.Commit();
        }
    }
}
