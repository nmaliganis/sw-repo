using sw.landmark.common.dtos.V1.Cqrs.EventPositions;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventPositionProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.EventPositionService
{
    public class DeleteHardEventPositionProcessor :
        IDeleteHardEventPositionProcessor,
        IRequestHandler<DeleteHardEventPositionCommand, BusinessResult<EventPositionDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEventPositionRepository _eventPositionRepository;

        public DeleteHardEventPositionProcessor(IUnitOfWork uOf, IEventPositionRepository eventPositionRepository)
        {
            _uOf = uOf;
            _eventPositionRepository = eventPositionRepository;
        }

        public async Task<BusinessResult<EventPositionDeletionUiModel>> Handle(DeleteHardEventPositionCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteHardEventPositionAsync(deleteCommand);
        }

        public async Task<BusinessResult<EventPositionDeletionUiModel>> DeleteHardEventPositionAsync(DeleteHardEventPositionCommand deleteCommand)
        {
            var bc = new BusinessResult<EventPositionDeletionUiModel>(new EventPositionDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var eventPosition = _eventPositionRepository.FindBy(deleteCommand.Id);
            if (eventPosition is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "EventPosition Id does not exist"));
                return bc;
            }

            Persist(eventPosition);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"EventPosition with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(EventPosition eventPosition)
        {
            _eventPositionRepository.Remove(eventPosition);
            _uOf.Commit();
        }
    }
}
