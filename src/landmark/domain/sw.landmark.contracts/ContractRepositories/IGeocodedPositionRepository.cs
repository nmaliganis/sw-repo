using sw.landmark.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.landmark.contracts.ContractRepositories
{
    public interface IGeocodedPositionRepository : IRepository<GeocodedPosition, long>
    {
        QueryResult<GeocodedPosition> FindAllActivePagedOf(int? pageNum, int? pageSize);
        GeocodedPosition FindActiveBy(long id);
    }
}
