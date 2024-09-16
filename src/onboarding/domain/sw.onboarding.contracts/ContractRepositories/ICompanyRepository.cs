using sw.onboarding.model.Companies;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.auth.contracts.ContractRepositories;

public interface ICompanyRepository : IRepository<Company, long>
{
  QueryResult<Company> FindAllActiveCompaniesPagedOf(int? pageNum, int? pageSize);
  int FindCountAllActiveCompanies();
  Company FindCompanyByName(string name);
  Company FindActiveById(long id);
}