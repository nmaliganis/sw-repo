using sw.auth.model.Roles;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.auth.contracts.ContractRepositories
{
    public interface IRoleRepository : IRepository<Role, long>
    {
        QueryResult<Role> FindAllActiveRolesPagedOf(int? pageNum, int? pageSize);
        int FindCountAllActiveRoles();

        Role FindRoleByName(string name);

        Role FindActiveById(long id);
    }
}