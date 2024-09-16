using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.DeviceModels;

internal class DeleteSoftDeviceModelHandler :
    IRequestHandler<DeleteSoftDeviceModelCommand, BusinessResult<DeviceModelDeletionUiModel>>
{
    private readonly IDeleteSoftDeviceModelProcessor _processor;

    public DeleteSoftDeviceModelHandler(IDeleteSoftDeviceModelProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<DeviceModelDeletionUiModel>> Handle(DeleteSoftDeviceModelCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteSoftDeviceModelAsync(deleteCommand);
    }
}
