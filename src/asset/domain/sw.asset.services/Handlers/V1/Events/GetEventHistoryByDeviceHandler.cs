using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.V1.EventProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Events;

internal class GetEventHistoryByDeviceHandler :
    IRequestHandler<GetEventHistoryByDeviceQuery, BusinessResult<PagedList<EventHistoryUiModel>>>
{
    private readonly IGetEventHistoryProcessor _processor;

    public GetEventHistoryByDeviceHandler(IGetEventHistoryProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<EventHistoryUiModel>>> Handle(GetEventHistoryByDeviceQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetEventHistoryByDeviceAsync(qry);
    }

}
