using sw.admin.contracts.ContractRepositories;
using sw.admin.model;
using sw.admin.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.admin.repository.DbContexts;

namespace sw.admin.repository.Repositories
{
    public class DepartmentPersonRoleRepository : RepositoryBase<DepartmentPersonRole, long>, IDepartmentPersonRoleRepository
    {
        public DepartmentPersonRoleRepository(swDbContext context)
            : base(context)
        {
        }

        public QueryResult<DepartmentPersonRole> FindAllActivePagedOf(int? pageNum, int? pageSize)
        {
            var qry = from dpr in Context.DepartmentPersonRoles
                      join dep in Context.Departments on dpr.DepartmentId equals dep.Id
                      join com in Context.Companies on dep.CompanyId equals com.Id
                      where dpr.Active
                         && dep.Active
                         && com.Active
                      select dpr;

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<DepartmentPersonRole>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<DepartmentPersonRole>(result, qry.Count(), (int)pageSize);
        }

        public DepartmentPersonRole FindActiveBy(long id)
        {
            var qry = from dpr in Context.DepartmentPersonRoles
                      join dep in Context.Departments on dpr.DepartmentId equals dep.Id
                      join com in Context.Companies on dep.CompanyId equals com.Id
                      where dpr.Active
                         && dep.Active
                         && com.Active
                         && dpr.Id == id
                      select dpr;

            return qry.FirstOrDefault();
        }
    }
}
