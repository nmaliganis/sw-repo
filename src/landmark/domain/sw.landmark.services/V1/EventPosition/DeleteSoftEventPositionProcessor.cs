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
    public class DeleteSoftEventPositionProcessor :
        IDeleteSoftEventPositionProcessor,
        IRequestHandler<DeleteSoftEventPositionCommand, BusinessResult<EventPositionDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEventPositionRepository _eventPositionRepository;

        public DeleteSoftEventPositionProcessor(IUnitOfWork uOf, IEventPositionRepository eventPositionRepository)
        {
            _uOf = uOf;
            _eventPositionRepository = eventPositionRepository;
        }

        public async Task<BusinessResult<EventPositionDeletionUiModel>> Handle(DeleteSoftEventPositionCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteSoftEventPositionAsync(deleteCommand);
        }

        public async Task<BusinessResult<EventPositionDeletionUiModel>> DeleteSoftEventPositionAsync(DeleteSoftEventPositionCommand deleteCommand)
        {
            var bc = new BusinessResult<EventPositionDeletionUiModel>(new EventPositionDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var eventPosition = _eventPositionRepository.FindBy(deleteCommand.Id);
            if (eventPosition is null || !eventPosition.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "EventPosition Id does not exist"));
                return bc;
            }

            eventPosition.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(eventPosition, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"EventPosition with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(EventPosition eventPosition, long id)
        {
            _eventPositionRepository.Save(eventPosition, id);
            _uOf.Commit();
        }
    }
}
