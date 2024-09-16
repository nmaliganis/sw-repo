using sw.admin.model;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.admin.contracts.ContractRepositories
{
    public interface IDepartmentRepository : IRepository<Department, long>
    {
        QueryResult<Department> FindAllActivePagedOf(int? pageNum, int? pageSize);
        Department FindActiveBy(long id);
        Department FindDepartmentByName(string name);
    }
}
