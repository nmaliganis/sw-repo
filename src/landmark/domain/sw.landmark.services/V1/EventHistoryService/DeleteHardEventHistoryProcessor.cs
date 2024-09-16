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
    public class DeleteHardEventHistoryProcessor :
        IDeleteHardEventHistoryProcessor,
        IRequestHandler<DeleteHardEventHistoryCommand, BusinessResult<EventHistoryDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IEventHistoryRepository _eventHistoryRepository;

        public DeleteHardEventHistoryProcessor(IUnitOfWork uOf, IEventHistoryRepository eventHistoryRepository)
        {
            _uOf = uOf;
            _eventHistoryRepository = eventHistoryRepository;
        }

        public async Task<BusinessResult<EventHistoryDeletionUiModel>> Handle(DeleteHardEventHistoryCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteHardEventHistoryAsync(deleteCommand);
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

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"EventHistory with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(EventHistory eventHistory)
        {
            _eventHistoryRepository.Remove(eventHistory);
            _uOf.Commit();
        }
    }
}
