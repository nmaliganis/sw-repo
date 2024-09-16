using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.DeviceModels;

internal class UpdateDeviceModelHandler :
    IRequestHandler<UpdateDeviceModelCommand, BusinessResult<DeviceModelModificationUiModel>>
{
    private readonly IUpdateDeviceModelProcessor _processor;

    public UpdateDeviceModelHandler(IUpdateDeviceModelProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<DeviceModelModificationUiModel>> Handle(UpdateDeviceModelCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateDeviceModelAsync(updateCommand);
    }
}
