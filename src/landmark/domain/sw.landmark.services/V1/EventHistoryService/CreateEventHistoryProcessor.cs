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
    public class CreateEventHistoryProcessor :
        ICreateEventHistoryProcessor,
        IRequestHandler<CreateEventHistoryCommand, BusinessResult<EventHistoryCreationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEventHistoryRepository _eventHistoryRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateEventHistoryProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IEventHistoryRepository eventHistoryRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _eventHistoryRepository = eventHistoryRepository;
        }

        public async Task<BusinessResult<EventHistoryCreationUiModel>> Handle(CreateEventHistoryCommand createCommand, CancellationToken cancellationToken)
        {
            return await CreateEventHistoryAsync(createCommand);
        }

        public async Task<BusinessResult<EventHistoryCreationUiModel>> CreateEventHistoryAsync(CreateEventHistoryCommand createCommand)
        {
            var bc = new BusinessResult<EventHistoryCreationUiModel>(new EventHistoryCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _eventHistoryRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "Company name already exists"));
            //}

            var eventHistory = _autoMapper.Map<EventHistory>(createCommand);
            eventHistory.Created(createCommand.CreatedById);

            Persist(eventHistory);

            var response = _autoMapper.Map<EventHistoryCreationUiModel>(eventHistory);
            response.Message = "EventHistory created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(EventHistory eventHistory)
        {
            _eventHistoryRepository.Add(eventHistory);
            _uOf.Commit();
        }
    }
}
