using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Simacards;

internal class GetSimcardByIdHandler :
    IRequestHandler<GetSimcardByIdQuery, BusinessResult<SimcardUiModel>>
{
    private readonly IGetSimcardByIdProcessor _processor;

    public GetSimcardByIdHandler(IGetSimcardByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SimcardUiModel>> Handle(GetSimcardByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetSimcardByIdAsync(qry.Id);
    }
}
