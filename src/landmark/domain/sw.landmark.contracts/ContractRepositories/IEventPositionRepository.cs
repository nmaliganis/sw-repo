using sw.landmark.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.landmark.contracts.ContractRepositories
{
    public interface IEventPositionRepository : IRepository<EventPosition, long>
    {
        QueryResult<EventPosition> FindAllActivePagedOf(int? pageNum, int? pageSize);
        EventPosition FindActiveBy(long id);
    }
}
