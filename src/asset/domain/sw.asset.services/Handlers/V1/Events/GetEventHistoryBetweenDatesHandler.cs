using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.V1.EventProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Events;

internal class GetEventHistoryBetweenDatesHandler :
    IRequestHandler<GetEventHistoryBetweenDatesQuery, BusinessResult<PagedList<EventHistoryUiModel>>>
{
    private readonly IGetEventHistoryProcessor _processor;

    public GetEventHistoryBetweenDatesHandler(IGetEventHistoryProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> Handle(GetEventHistoryBetweenDatesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetEventHistoryBetweenDatesAsync(qry);
    }
}
