using sw.admin.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.admin.contracts.ContractRepositories
{
    public interface IPersonRepository : IRepository<Person, long>
    {
        QueryResult<Person> FindAllActivePagedOf(int? pageNum, int? pageSize);
        Person FindActiveBy(long id);
        Person FindPersonByEmail(string email);
    }
}
