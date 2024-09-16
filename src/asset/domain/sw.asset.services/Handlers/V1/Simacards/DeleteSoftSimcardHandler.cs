using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Simacards;

internal class DeleteSoftSimcardHandler :
    IRequestHandler<DeleteSoftSimcardCommand, BusinessResult<SimcardDeletionUiModel>>
{
    private readonly IDeleteSoftSimcardProcessor _processor;

    public DeleteSoftSimcardHandler(IDeleteSoftSimcardProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<SimcardDeletionUiModel>> Handle(DeleteSoftSimcardCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteSoftSimcardAsync(deleteCommand);
    }
}
