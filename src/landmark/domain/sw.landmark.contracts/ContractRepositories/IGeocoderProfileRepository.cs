using sw.landmark.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.landmark.contracts.ContractRepositories
{
    public interface IGeocoderProfileRepository : IRepository<GeocoderProfile, long>
    {
        QueryResult<GeocoderProfile> FindAllActivePagedOf(int? pageNum, int? pageSize);
        GeocoderProfile FindActiveBy(long id);
    }
}
