using sw.landmark.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.landmark.contracts.ContractRepositories
{
    public interface IEventHistoryRepository : IRepository<EventHistory, long>
    {
        QueryResult<EventHistory> FindAllActivePagedOf(int? pageNum, int? pageSize);
        EventHistory FindActiveBy(long id);
    }
}
