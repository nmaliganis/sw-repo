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
    public class DeleteSoftDeviceModelProcessor :
        IDeleteSoftDeviceModelProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDeviceModelRepository _deviceModelRepository;

        public DeleteSoftDeviceModelProcessor(IUnitOfWork uOf, IDeviceModelRepository deviceModelRepository)
        {
            _uOf = uOf;
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<BusinessResult<DeviceModelDeletionUiModel>> DeleteSoftDeviceModelAsync(DeleteSoftDeviceModelCommand deleteCommand)
        {
            var bc = new BusinessResult<DeviceModelDeletionUiModel>(new DeviceModelDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var deviceModel = _deviceModelRepository.FindBy(deleteCommand.Id);
            if (deviceModel is null || !deviceModel.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "DeviceModel Id does not exist"));
                return bc;
            }

            deviceModel.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(deviceModel, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"DeviceModel with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(DeviceModel deviceModel, long id)
        {
            _deviceModelRepository.Save(deviceModel, id);
            _uOf.Commit();
        }
    }
}
