using sw.landmark.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.landmark.contracts.ContractRepositories
{
    public interface ILandmarkRepository : IRepository<Landmark, long>
    {
        QueryResult<Landmark> FindAllActivePagedOf(int? pageNum, int? pageSize);
        Landmark FindActiveBy(long id);
    }
}
