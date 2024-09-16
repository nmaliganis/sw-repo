using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.EventProcessors
{
    public interface IGetEventHistoryProcessor
    {
        Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryAsync(GetEventHistoryQuery qry);
        Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryByDeviceAsync(GetEventHistoryByDeviceQuery qry);
        Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryBetweenDatesAsync(GetEventHistoryBetweenDatesQuery qry);
        Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryBetweenDatesForDeviceAsync(GetEventHistoryBetweenDatesForDeviceQuery qry);
        Task<BusinessResult<PagedList<EventHistoryUiModel>>> GetEventHistoryNLastRecordsAsync(GetEventHistoryNLastRecordsQuery qry);
    }
}
