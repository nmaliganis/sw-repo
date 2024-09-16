using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;
using sw.asset.model.Devices;

namespace sw.asset.services.V1.DeviceService;

public class DeleteHardDeviceProcessor :
  IDeleteHardDeviceProcessor
{
  private readonly IUnitOfWork _uOf;
  private readonly IDeviceRepository _deviceRepository;

  public DeleteHardDeviceProcessor(IUnitOfWork uOf, IDeviceRepository deviceRepository)
  {
    _uOf = uOf;
    _deviceRepository = deviceRepository;
  }

  public async Task<BusinessResult<DeviceDeletionUiModel>> DeleteHardDeviceAsync(DeleteHardDeviceCommand deleteCommand)
  {
    var bc = new BusinessResult<DeviceDeletionUiModel>(new DeviceDeletionUiModel());

    if (deleteCommand is null)
    {
      bc.AddBrokenRule(new BusinessError(null));
    }

    var device = _deviceRepository.FindBy(deleteCommand.Id);
    if (device is null)
    {
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Device Id does not exist"));
      return bc;
    }

    Persist(device);

    bc.Model.Id = deleteCommand.Id;
    bc.Model.Successful = true;
    bc.Model.Hard = true;
    bc.Model.Message = $"Device with id: {deleteCommand.Id} has been hard deleted successfully.";

    return await Task.FromResult(bc);
  }

  private void Persist(Device Device)
  {
    _deviceRepository.Remove(Device);
    _uOf.Commit();
  }
}