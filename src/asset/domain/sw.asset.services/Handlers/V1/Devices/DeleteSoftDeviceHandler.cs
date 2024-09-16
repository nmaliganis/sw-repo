using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.asset.contracts.V1.DeviceProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Devices;

internal class DeleteSoftDeviceHandler :
    IRequestHandler<DeleteSoftDeviceCommand, BusinessResult<DeviceDeletionUiModel>>
{
    private readonly IDeleteSoftDeviceProcessor _processor;

    public DeleteSoftDeviceHandler(IDeleteSoftDeviceProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<DeviceDeletionUiModel>> Handle(DeleteSoftDeviceCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteSoftDeviceAsync(deleteCommand);
    }
}
