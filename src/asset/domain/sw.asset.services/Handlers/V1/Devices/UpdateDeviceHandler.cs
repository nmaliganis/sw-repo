using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Devices;

internal class UpdateDeviceHandler :
    IRequestHandler<UpdateDeviceCommand, BusinessResult<DeviceModificationUiModel>>
{
  private readonly IUpdateDeviceProcessor _processor;

  public UpdateDeviceHandler(IUpdateDeviceProcessor processor)
  {
    _processor = processor;
  }

  public async Task<BusinessResult<DeviceModificationUiModel>> Handle(UpdateDeviceCommand updateCommand, CancellationToken cancellationToken)
  {
    return await _processor.UpdateDeviceAsync(updateCommand);
  }
}