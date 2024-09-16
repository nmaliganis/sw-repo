using sw.landmark.common.dtos.V1.Cqrs.EventPositions;
using sw.landmark.common.dtos.V1.Vms.EventPositions;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.EventPositionProcessors
{
    public interface IGetEventPositionsProcessor
    {
        Task<BusinessResult<PagedList<EventPositionUiModel>>> GetEventPositionsAsync(GetEventPositionsQuery qry);
    }
}
