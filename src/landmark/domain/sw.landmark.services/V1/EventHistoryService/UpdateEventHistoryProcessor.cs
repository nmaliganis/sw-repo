using sw.landmark.common.dtos.V1.Cqrs.EventHistories;
using sw.landmark.common.dtos.V1.Vms.EventHistories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventHistoryProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.EventHistoryService
{
    public class UpdateEventHistoryProcessor :
        IUpdateEventHistoryProcessor,
        IRequestHandler<UpdateEventHistoryCommand, BusinessResult<EventHistoryModificationUiModel>>
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

        public async Task<BusinessResult<EventHistoryModificationUiModel>> Handle(UpdateEventHistoryCommand updateCommand, CancellationToken cancellationToken)
        {
            return await UpdateEventHistoryAsync(updateCommand);
        }

        public async Task<BusinessResult<EventHistoryModificationUiModel>> UpdateEventHistoryAsync(UpdateEventHistoryCommand updateCommand)
        {
            var bc = new BusinessResult<EventHistoryModificationUiModel>(new EventHistoryModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var eventHistory = _eventHistoryRepository.FindBy(updateCommand.Id);
            if (eventHistory is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "EventHistory Id does not exist"));
                return bc;
            }

            var modifiedEventHistory = _autoMapper.Map<EventHistory>(updateCommand);
            eventHistory.Modified(updateCommand.ModifiedById, modifiedEventHistory);

            Persist(eventHistory, updateCommand.Id);

            var response = _autoMapper.Map<EventHistoryModificationUiModel>(eventHistory);
            response.Message = $"EventHistory id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(EventHistory eventHistory, long id)
        {
            _eventHistoryRepository.Save(eventHistory, id);
            _uOf.Commit();
        }
    }
}
