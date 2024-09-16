using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.asset.model.Devices.Simcards;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;

namespace sw.asset.services.V1.SimcardService;

public class DeleteHardSimcardProcessor :
  IDeleteHardSimcardProcessor,
  IRequestHandler<DeleteHardSimcardCommand, BusinessResult<SimcardDeletionUiModel>>
{
  private readonly IUnitOfWork _uOf;
  private readonly ISimcardRepository _simcardRepository;

  public DeleteHardSimcardProcessor(IUnitOfWork uOf, ISimcardRepository simcardRepository)
  {
    _uOf = uOf;
    _simcardRepository = simcardRepository;
  }

  public async Task<BusinessResult<SimcardDeletionUiModel>> Handle(DeleteHardSimcardCommand deleteCommand, CancellationToken cancellationToken)
  {
    return await DeleteHardSimcardAsync(deleteCommand);
  }

  public async Task<BusinessResult<SimcardDeletionUiModel>> DeleteHardSimcardAsync(DeleteHardSimcardCommand deleteCommand)
  {
    var bc = new BusinessResult<SimcardDeletionUiModel>(new SimcardDeletionUiModel());

    if (deleteCommand is null)
    {
      bc.AddBrokenRule(new BusinessError(null));
    }

    var simcard = _simcardRepository.FindBy(deleteCommand!.Id);
    if (simcard is null)
    {
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Simcard Id does not exist"));
      return bc;
    }

    Persist(simcard);

    bc.Model.Message = $"Simcard with id: {deleteCommand.Id} has been hard deleted successfully.";

    return await Task.FromResult(bc);
  }

  private void Persist(Simcard simcard)
  {
    _simcardRepository.Remove(simcard);
    _uOf.Commit();
  }
}//Class : DeleteHardSimcardProcessor