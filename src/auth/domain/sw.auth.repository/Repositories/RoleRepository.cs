using sw.auth.contracts.ContractRepositories;
using sw.auth.model.Roles;
using sw.auth.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;

namespace sw.auth.repository.Repositories;

public class RoleRepository : RepositoryBase<Role, long>, IRoleRepository
{
  public RoleRepository(ISession session)
    : base(session)
  {
  }

  public QueryResult<Role> FindAllActiveRolesPagedOf(int? pageNum, int? pageSize)
  {
    var query = Session.QueryOver<Role>();

    if (pageNum == -1 & pageSize == -1)
    {
      return new QueryResult<Role>(query?
        .Where(r => r.Active == true)
        .WhereNot(r => r.Name == "SU")
        .List().AsQueryable());
    }

    return new QueryResult<Role>(query
          .Where(r => r.Active == true)
          .WhereNot(r => r.Name == "SU")
          .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
          .Take((int)pageSize).List().AsQueryable(),
        query.ToRowCountQuery().RowCount(),
        (int)pageSize)
      ;
  }

  public int FindCountAllActiveRoles()
  {
    int count;

    count = Session
      .CreateCriteria<Role>()
      .Add(Expression.Eq("Active", true))
      .SetProjection(
        Projections.Count(Projections.Id())
      )
      .UniqueResult<int>();

    return count;
  }

  public Role FindRoleByName(string name)
  {
    return
      (Role)
      Session.CreateCriteria(typeof(Role))
        .Add(Expression.Eq("Name", name))
        .Add(Expression.Eq("Active", true))
        .SetCacheable(true)
        .SetCacheMode(CacheMode.Normal)
        .UniqueResult()
      ;
  }
  public Role FindActiveById(long id)
  {
    Role role = null;

    return this.Session.QueryOver<Role>(() => role)
      .Where(() => role.Active == true)
      .And(() => role.Id == id)
      .Cacheable()
      .CacheMode(CacheMode.Normal)
      .SetFlushMode(FlushMode.Manual)
      .SingleOrDefault();
  }
}