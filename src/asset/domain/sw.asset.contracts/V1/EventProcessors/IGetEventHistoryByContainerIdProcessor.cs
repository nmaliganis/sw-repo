using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.EventProcessors
{
    public interface IGetEventHistoryByContainerIdProcessor
    {
        Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryByContainerIdAsync(GetEventHistoryByContainerIdQuery qry, CancellationToken cancellationToken);
        Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryBetweenDatesForContainerAsync(GetEventHistoryBetweenDatesForContainerQuery qry, CancellationToken cancellationToken);
    }
}
