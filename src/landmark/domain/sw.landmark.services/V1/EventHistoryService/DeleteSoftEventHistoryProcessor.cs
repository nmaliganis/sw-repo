using sw.landmark.common.dtos.V1.Cqrs.EventHistories;
using sw.landmark.common.dtos.V1.Vms.EventHistories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventHistoryProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.EventHistoryService
{
    public class DeleteSoftEventHistoryProcessor :
        IDeleteSoftEventHistoryProcessor,
        IRequestHandler<DeleteSoftEventHistoryCommand, BusinessResult<EventHistoryDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEventHistoryRepository _eventHistoryRepository;

        public DeleteSoftEventHistoryProcessor(IUnitOfWork uOf, IEventHistoryRepository eventHistoryRepository)
        {
            _uOf = uOf;
            _eventHistoryRepository = eventHistoryRepository;
        }

        public async Task<BusinessResult<EventHistoryDeletionUiModel>> Handle(DeleteSoftEventHistoryCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteSoftEventHistoryAsync(deleteCommand);
        }

        public async Task<BusinessResult<EventHistoryDeletionUiModel>> DeleteSoftEventHistoryAsync(DeleteSoftEventHistoryCommand deleteCommand)
        {
            var bc = new BusinessResult<EventHistoryDeletionUiModel>(new EventHistoryDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var eventHistory = _eventHistoryRepository.FindBy(deleteCommand.Id);
            if (eventHistory is null || !eventHistory.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "EventHistory Id does not exist"));
                return bc;
            }

            eventHistory.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(eventHistory, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"EventHistory with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(EventHistory eventHistory, long id)
        {
            _eventHistoryRepository.Save(eventHistory, id);
            _uOf.Commit();
        }
    }
}
