using sw.landmark.common.dtos.V1.Cqrs.EventPositions;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventPositionProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.EventPositionService
{
    public class CreateEventPositionProcessor :
        ICreateEventPositionProcessor,
        IRequestHandler<CreateEventPositionCommand, BusinessResult<EventPositionCreationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEventPositionRepository _eventPositionRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateEventPositionProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IEventPositionRepository eventPositionRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _eventPositionRepository = eventPositionRepository;
        }

        public async Task<BusinessResult<EventPositionCreationUiModel>> Handle(CreateEventPositionCommand createCommand, CancellationToken cancellationToken)
        {
            return await CreateEventPositionAsync(createCommand);
        }

        public async Task<BusinessResult<EventPositionCreationUiModel>> CreateEventPositionAsync(CreateEventPositionCommand createCommand)
        {
            var bc = new BusinessResult<EventPositionCreationUiModel>(new EventPositionCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _eventPositionRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "Company name already exists"));
            //}

            var eventPosition = _autoMapper.Map<EventPosition>(createCommand);
            eventPosition.Created(createCommand.CreatedById);

            Persist(eventPosition);

            var response = _autoMapper.Map<EventPositionCreationUiModel>(eventPosition);
            response.Message = "EventPosition created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(EventPosition eventPosition)
        {
            _eventPositionRepository.Add(eventPosition);
            _uOf.Commit();
        }
    }
}
