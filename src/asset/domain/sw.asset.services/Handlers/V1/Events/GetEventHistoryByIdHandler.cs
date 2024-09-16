using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.V1.EventProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Events;

internal class GetEventHistoryByIdHandler :
    IRequestHandler<GetEventHistoryByIdQuery, BusinessResult<EventHistoryUiModel>>
{
    private readonly IGetEventHistoryByIdProcessor _processor;

    public GetEventHistoryByIdHandler(IGetEventHistoryByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<EventHistoryUiModel>> Handle(GetEventHistoryByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetEventHistoryByIdAsync(qry.Id);
    }
}
