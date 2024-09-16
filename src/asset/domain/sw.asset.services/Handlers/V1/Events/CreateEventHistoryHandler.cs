using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.V1.EventProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Events;

internal class CreateEventHistoryHandler :
    IRequestHandler<CreateEventHistoryCommand, BusinessResult<EventHistoryUiModel>>
{
    private readonly ICreateEventHistoryProcessor _processor;

    public CreateEventHistoryHandler(ICreateEventHistoryProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<EventHistoryUiModel>> Handle(CreateEventHistoryCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateEventHistoryAsync(createCommand.DeviceImei, createCommand);
    }
}