using sw.landmark.common.dtos.V1.Cqrs.EventPositions;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.EventPositionProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.EventPositionService
{
    public class GetEventPositionByIdProcessor :
        IGetEventPositionByIdProcessor,
        IRequestHandler<GetEventPositionByIdQuery, BusinessResult<EventPositionUiModel>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IEventPositionRepository _eventPositionRepository;
        public GetEventPositionByIdProcessor(IEventPositionRepository eventPositionRepository, IAutoMapper autoMapper)
        {
            _eventPositionRepository = eventPositionRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<EventPositionUiModel>> GetEventPositionByIdAsync(long id)
        {
            var bc = new BusinessResult<EventPositionUiModel>(new EventPositionUiModel());

            var eventPosition = _eventPositionRepository.FindActiveBy(id);
            if (eventPosition is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "EventPosition Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<EventPositionUiModel>(eventPosition);
            response.Message = $"EventPosition id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<EventPositionUiModel>> Handle(GetEventPositionByIdQuery qry, CancellationToken cancellationToken)
        {
            return await GetEventPositionByIdAsync(qry.Id);
        }
    }
}
