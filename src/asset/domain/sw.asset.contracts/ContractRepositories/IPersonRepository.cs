using sw.asset.model;
using sw.asset.model.Persons;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories;

public interface IPersonRepository : IRepository<Person, long>
{
  QueryResult<Person> FindAllActivePagedOf(int? pageNum, int? pageSize);
  Person FindOneByEmail(string email);
  Person FindOneActiveById(long id);
  int FindCountAllActive();
}