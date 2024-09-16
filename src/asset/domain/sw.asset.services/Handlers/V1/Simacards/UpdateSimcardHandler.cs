using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Simacards;

internal class UpdateSimcardHandler :
    IRequestHandler<UpdateSimcardCommand, BusinessResult<SimcardModificationUiModel>>
{
  private readonly IUpdateSimcardProcessor _processor;

  public UpdateSimcardHandler(IUpdateSimcardProcessor processor)
  {
    _processor = processor;
  }

  public async Task<BusinessResult<SimcardModificationUiModel>> Handle(UpdateSimcardCommand updateCommand, CancellationToken cancellationToken)
  {
    return await _processor.UpdateSimcardAsync(updateCommand);
  }
}