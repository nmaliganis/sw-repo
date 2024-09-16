using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.V1.EventProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Events;

internal class UpdateEventHistoryHandler :
    IRequestHandler<UpdateEventHistoryCommand, BusinessResult<EventHistoryModificationUiModel>>
{
    private readonly IUpdateEventHistoryProcessor _processor;

    public UpdateEventHistoryHandler(IUpdateEventHistoryProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<EventHistoryModificationUiModel>> Handle(UpdateEventHistoryCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateEventHistoryAsync(updateCommand);
    }
}
