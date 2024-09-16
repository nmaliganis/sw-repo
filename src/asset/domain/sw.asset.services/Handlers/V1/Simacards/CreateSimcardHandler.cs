using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Simacards;

internal class CreateSimcardHandler :
    IRequestHandler<CreateSimcardCommand, BusinessResult<SimcardCreationUiModel>>
{
    private readonly ICreateSimcardProcessor _processor;

    public CreateSimcardHandler(ICreateSimcardProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SimcardCreationUiModel>> Handle(CreateSimcardCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateSimcardAsync(createCommand);
    }
}