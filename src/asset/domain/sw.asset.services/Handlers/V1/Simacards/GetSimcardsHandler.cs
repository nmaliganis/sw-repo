using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.Simacards;

internal class GetSimcardsHandler :
    IRequestHandler<GetSimcardsQuery, BusinessResult<PagedList<SimcardUiModel>>>
{
    private readonly IGetSimcardsProcessor _processor;

    public GetSimcardsHandler(IGetSimcardsProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<SimcardUiModel>>> Handle(GetSimcardsQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetSimcardsAsync(qry);
    }
}
