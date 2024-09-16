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
    public class UpdateEventPositionProcessor :
        IUpdateEventPositionProcessor,
        IRequestHandler<UpdateEventPositionCommand, BusinessResult<EventPositionModificationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEventPositionRepository _eventPositionRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateEventPositionProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IEventPositionRepository eventPositionRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _eventPositionRepository = eventPositionRepository;
        }

        public async Task<BusinessResult<EventPositionModificationUiModel>> Handle(UpdateEventPositionCommand updateCommand, CancellationToken cancellationToken)
        {
            return await UpdateEventPositionAsync(updateCommand);
        }

        public async Task<BusinessResult<EventPositionModificationUiModel>> UpdateEventPositionAsync(UpdateEventPositionCommand updateCommand)
        {
            var bc = new BusinessResult<EventPositionModificationUiModel>(new EventPositionModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var eventPosition = _eventPositionRepository.FindBy(updateCommand.Id);
            if (eventPosition is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "EventPosition Id does not exist"));
                return bc;
            }

            var modifiedEventPosition = _autoMapper.Map<EventPosition>(updateCommand);
            eventPosition.Modified(updateCommand.ModifiedById, modifiedEventPosition);

            Persist(eventPosition, updateCommand.Id);

            var response = _autoMapper.Map<EventPositionModificationUiModel>(eventPosition);
            response.Message = $"EventPosition id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(EventPosition eventPosition, long id)
        {
            _eventPositionRepository.Save(eventPosition, id);
            _uOf.Commit();
        }
    }
}
