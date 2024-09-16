using sw.asset.model.Companies;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories;

public interface ICompanyRepository : IRepository<Company, long>
{
    QueryResult<Company> FindAllActivePagedOf(int? pageNum, int? pageSize);
    Company FindActiveById(long id);
    Company FindActiveByName(string name);
}