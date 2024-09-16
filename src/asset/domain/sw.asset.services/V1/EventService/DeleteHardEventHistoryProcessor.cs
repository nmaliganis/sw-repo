using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.EventProcessors;
using sw.asset.model.Events;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.asset.services.V1.EventService;

public class DeleteHardEventHistoryProcessor :
    IDeleteHardEventHistoryProcessor
{
  private readonly IUnitOfWork _uOf;
  private readonly IEventHistoryRepository _eventHistoryRepository;

  public DeleteHardEventHistoryProcessor(IUnitOfWork uOf, IEventHistoryRepository eventHistoryRepository)
  {
    _uOf = uOf;
    _eventHistoryRepository = eventHistoryRepository;
  }

  public async Task<BusinessResult<EventHistoryDeletionUiModel>> DeleteHardEventHistoryAsync(DeleteHardEventHistoryCommand deleteCommand)
  {
    var bc = new BusinessResult<EventHistoryDeletionUiModel>(new EventHistoryDeletionUiModel());

    if (deleteCommand is null)
    {
      bc.AddBrokenRule(new BusinessError(null));
    }

    var eventHistory = _eventHistoryRepository.FindBy(deleteCommand.Id);
    if (eventHistory is null)
    {
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "EventHistory Id does not exist"));
      return bc;
    }

    Persist(eventHistory);

    bc.Model.Successful = true;
    bc.Model.Message = $"EventHistory with id: {deleteCommand.Id} has been deleted successfully.";

    return await Task.FromResult(bc);
  }

  private void Persist(EventHistory eventHistory)
  {
    _eventHistoryRepository.Remove(eventHistory);
    _uOf.Commit();
  }
}// Class: DeleteHardEventHistoryProcessor
