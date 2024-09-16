using sw.admin.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.admin.contracts.ContractRepositories
{
    public interface ICompanyRepository : IRepository<Company, long>
    {
        QueryResult<Company> FindAllActivePagedOf(int? pageNum, int? pageSize);
        Company FindActiveBy(long id);

        Company FindCompanyByName(string name);
    }
}
