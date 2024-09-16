using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.EventProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.asset.services.V1.EventService
{
  public class GetEventHistoryByIdProcessor :
        IGetEventHistoryByIdProcessor
  {
    private readonly IAutoMapper _autoMapper;
    private readonly IEventHistoryRepository _eventHistoryRepository;
    public GetEventHistoryByIdProcessor(IEventHistoryRepository eventHistoryRepository, IAutoMapper autoMapper)
    {
      _eventHistoryRepository = eventHistoryRepository;
      _autoMapper = autoMapper;
    }

    public async Task<BusinessResult<EventHistoryUiModel>> GetEventHistoryByIdAsync(long id)
    {
      var bc = new BusinessResult<EventHistoryUiModel>(new EventHistoryUiModel());

      return await Task.FromResult(bc);
    }
  }
}
