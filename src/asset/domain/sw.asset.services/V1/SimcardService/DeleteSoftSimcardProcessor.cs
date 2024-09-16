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

public class DeleteSoftSimcardProcessor :
  IDeleteSoftSimcardProcessor,
  IRequestHandler<DeleteSoftSimcardCommand, BusinessResult<SimcardDeletionUiModel>>
{
  private readonly IUnitOfWork _uOf;
  private readonly ISimcardRepository _simcardRepository;

  public DeleteSoftSimcardProcessor(IUnitOfWork uOf, ISimcardRepository simcardRepository)
  {
    _uOf = uOf;
    _simcardRepository = simcardRepository;
  }

  public async Task<BusinessResult<SimcardDeletionUiModel>> Handle(DeleteSoftSimcardCommand deleteCommand, CancellationToken cancellationToken)
  {
    return await DeleteSoftSimcardAsync(deleteCommand);
  }

  public async Task<BusinessResult<SimcardDeletionUiModel>> DeleteSoftSimcardAsync(DeleteSoftSimcardCommand deleteCommand)
  {
    var bc = new BusinessResult<SimcardDeletionUiModel>(new SimcardDeletionUiModel());

    if (deleteCommand is null)
    {
      bc.AddBrokenRule(new BusinessError(null));
    }

    var simcard = _simcardRepository.FindBy(deleteCommand!.Id);
    if (simcard is null || !simcard.Active)
    {
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Simcard Id does not exist"));
      return bc;
    }

    simcard.DeleteWithAudit(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

    Persist(simcard, deleteCommand.Id);

    bc.Model.Id = deleteCommand.Id;
    bc.Model.Active = false;
    bc.Model.Message = $"Simcard with id: {deleteCommand.Id} has been deleted successfully.";

    return await Task.FromResult(bc);
  }

  private void Persist(Simcard simcard, long id)
  {
    _simcardRepository.Save(simcard, id);
    _uOf.Commit();
  }
}//Class : DeleteSoftSimcardProcessor