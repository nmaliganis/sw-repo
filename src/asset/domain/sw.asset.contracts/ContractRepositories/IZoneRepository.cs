using sw.asset.model.Companies;
using sw.asset.model.Companies.Zones;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories;

public interface IZoneRepository : IRepository<Zone, long>
{
    QueryResult<Zone> FindAllActivePagedOf(long companyId, int? pageNum, int? pageSize);
    Zone FindActiveById(long id);
    Zone FindActiveByName(string name);
}