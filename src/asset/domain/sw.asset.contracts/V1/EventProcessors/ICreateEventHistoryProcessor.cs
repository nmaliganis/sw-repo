using sw.asset.common.dtos.Cqrs.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.EventProcessors;

public interface ICreateEventHistoryProcessor
{
    Task<BusinessResult<EventHistoryUiModel>> CreateEventHistoryAsync(string deviceImei, CreateEventHistoryCommand createCommand);
}