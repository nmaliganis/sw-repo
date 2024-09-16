using sw.landmark.common.dtos.V1.Cqrs.EventHistories;
using sw.landmark.common.dtos.V1.Vms.EventHistories;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventHistoryProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.EventHistoryService
{
    public class GetEventHistoryByIdProcessor :
        IGetEventHistoryByIdProcessor,
        IRequestHandler<GetEventHistoryByIdQuery, BusinessResult<EventHistoryUiModel>>
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

            var eventHistory = _eventHistoryRepository.FindActiveBy(id);
            if (eventHistory is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "EventHistory Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<EventHistoryUiModel>(eventHistory);
            response.Message = $"EventHistory id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<EventHistoryUiModel>> Handle(GetEventHistoryByIdQuery qry, CancellationToken cancellationToken)
        {
            return await GetEventHistoryByIdAsync(qry.Id);
        }
    }
}
