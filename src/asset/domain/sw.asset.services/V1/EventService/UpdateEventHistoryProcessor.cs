using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.EventProcessors;
using sw.asset.model.Events;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System;
using System.Threading.Tasks;

namespace sw.asset.services.V1.EventService;

public class UpdateEventHistoryProcessor :
    IUpdateEventHistoryProcessor
{
    private readonly IUnitOfWork _uOf;
    private readonly IEventHistoryRepository _eventHistoryRepository;
    private readonly IAutoMapper _autoMapper;
    public UpdateEventHistoryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
        IEventHistoryRepository eventHistoryRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _eventHistoryRepository = eventHistoryRepository;
    }

    public async Task<BusinessResult<EventHistoryModificationUiModel>> UpdateEventHistoryAsync(UpdateEventHistoryCommand updateCommand)
    {
        var bc = new BusinessResult<EventHistoryModificationUiModel>(new EventHistoryModificationUiModel());

        if (updateCommand.IsNull())
        {
            bc.AddBrokenRule(new BusinessError(null));
        }

        if (updateCommand != null)
        {
            var eventHistory = _eventHistoryRepository.FindBy(updateCommand.Id);
            if (eventHistory is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "EventHistory Id does not exist"));
                return bc;
            }

            var modifiedEventHistory = _autoMapper.Map<EventHistory>(updateCommand);
            //eventHistory.Modified(updateCommand.ModifiedById, modifiedEventHistory);


            var response = _autoMapper.Map<EventHistoryModificationUiModel>(eventHistory);
            response.Message = $"EventHistory id: {response.Id} updated successfully";

            bc.Model = response;
        }

        return await Task.FromResult(bc);
    }

    private void Persist(EventHistory eventHistory, Guid id)
    {
        _eventHistoryRepository.Save(eventHistory, id);
        _uOf.Commit();
    }
}