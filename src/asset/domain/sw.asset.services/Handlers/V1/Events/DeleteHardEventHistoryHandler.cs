using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.V1.EventProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Events;

internal class DeleteHardEventHistoryHandler :
    IRequestHandler<DeleteHardEventHistoryCommand, BusinessResult<EventHistoryDeletionUiModel>>
{
    private readonly IDeleteHardEventHistoryProcessor _processor;

    public DeleteHardEventHistoryHandler(IDeleteHardEventHistoryProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<EventHistoryDeletionUiModel>> Handle(DeleteHardEventHistoryCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteHardEventHistoryAsync(deleteCommand);
    }
}
