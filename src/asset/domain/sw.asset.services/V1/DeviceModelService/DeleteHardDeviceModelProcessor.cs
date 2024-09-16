using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.asset.model.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.asset.services.V1.DeviceModelService
{
    public class DeleteHardDeviceModelProcessor :
        IDeleteHardDeviceModelProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDeviceModelRepository _deviceModelRepository;

        public DeleteHardDeviceModelProcessor(IUnitOfWork uOf, IDeviceModelRepository deviceModelRepository)
        {
            _uOf = uOf;
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<BusinessResult<DeviceModelDeletionUiModel>> DeleteHardDeviceModelAsync(DeleteHardDeviceModelCommand deleteCommand)
        {
            var bc = new BusinessResult<DeviceModelDeletionUiModel>(new DeviceModelDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var deviceModel = _deviceModelRepository.FindBy(deleteCommand.Id);
            if (deviceModel is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "DeviceModel Id does not exist"));
                return bc;
            }

            Persist(deviceModel);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"DeviceModel with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(DeviceModel deviceModel)
        {
            _deviceModelRepository.Remove(deviceModel);
            _uOf.Commit();
        }
    }
}
