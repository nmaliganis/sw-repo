using sw.admin.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.admin.contracts.ContractRepositories
{
    public interface IDepartmentPersonRoleRepository : IRepository<DepartmentPersonRole, long>
    {
        QueryResult<DepartmentPersonRole> FindAllActivePagedOf(int? pageNum, int? pageSize);
        DepartmentPersonRole FindActiveBy(long id);
    }
}
