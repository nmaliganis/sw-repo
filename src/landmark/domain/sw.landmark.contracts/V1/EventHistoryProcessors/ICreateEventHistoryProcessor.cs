using sw.landmark.common.dtos.V1.Cqrs.EventHistories;
using sw.landmark.common.dtos.V1.Vms.EventHistories;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.landmark.contracts.V1.EventHistoryProcessors
{
    public interface ICreateEventHistoryProcessor
    {
        Task<BusinessResult<EventHistoryCreationUiModel>> CreateEventHistoryAsync(CreateEventHistoryCommand createCommand);
    }
}
