using sw.auth.model.Companies;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.auth.contracts.ContractRepositories;

public interface ICompanyRepository : IRepository<Company, long>
{
    QueryResult<Company> FindAllActiveCompaniesPagedOf(int? pageNum, int? pageSize);
    QueryResult<Company> FindAllActiveCompaniesByUserPagedOf(long companyId, int? pageNum, int? pageSize);
    int FindCountAllActiveCompanies();
    Company FindCompanyByName(string name);
    Company FindActiveById(long id);
}