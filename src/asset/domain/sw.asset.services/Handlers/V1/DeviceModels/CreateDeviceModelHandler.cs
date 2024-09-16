using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.asset.contracts.V1.DeviceModelProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.DeviceModels;

internal class CreateDeviceModelHandler :
    IRequestHandler<CreateDeviceModelCommand, BusinessResult<DeviceModelCreationUiModel>>
{
    private readonly ICreateDeviceModelProcessor _processor;

    public CreateDeviceModelHandler(ICreateDeviceModelProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<DeviceModelCreationUiModel>> Handle(CreateDeviceModelCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateDeviceModelAsync(createCommand);
    }
}
