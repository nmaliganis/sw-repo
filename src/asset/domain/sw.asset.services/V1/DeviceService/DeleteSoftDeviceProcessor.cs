using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.Devices;

namespace sw.asset.services.V1.DeviceService
{
    public class DeleteSoftDeviceProcessor :
        IDeleteSoftDeviceProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDeviceRepository _deviceRepository;

        public DeleteSoftDeviceProcessor(IUnitOfWork uOf, IDeviceRepository deviceRepository)
        {
            _uOf = uOf;
            _deviceRepository = deviceRepository;
        }

        public async Task<BusinessResult<DeviceDeletionUiModel>> DeleteSoftDeviceAsync(DeleteSoftDeviceCommand deleteCommand)
        {
            var bc = new BusinessResult<DeviceDeletionUiModel>(new DeviceDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var device = _deviceRepository.FindBy(deleteCommand.Id);
            if (device is null || !device.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Device Id does not exist"));
                return bc;
            }

            device.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(device, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"Device with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Device device, long id)
        {
            _deviceRepository.Save(device, id);
            _uOf.Commit();
        }
    }
}
