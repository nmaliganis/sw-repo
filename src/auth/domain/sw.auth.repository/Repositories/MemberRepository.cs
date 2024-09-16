using sw.auth.contracts.ContractRepositories;
using sw.auth.model.Members;
using sw.auth.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System.Collections.Generic;
using System.Linq;

namespace sw.auth.repository.Repositories;

public class MemberRepository : RepositoryBase<Member, long>, IMemberRepository
{
  public MemberRepository(ISession session)
    : base(session)
  {
  }

  public QueryResult<Member> FindAllActiveMembersPagedOf(int? pageNum, int? pageSize)
  {
    var query = Session.QueryOver<Member>();

    if (pageNum == -1 & pageSize == -1)
    {
      return new QueryResult<Member>(query?
        .Where(r => r.Active == true)
        .List().AsQueryable());
    }

    return new QueryResult<Member>(query
          .Where(r => r.Active == true)
          .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
          .Take((int)pageSize).List().AsQueryable(),
        query.ToRowCountQuery().RowCount(),
        (int)pageSize)
      ;
  }

  public int FindCountAllActiveMembers()
  {
    var count = Session
      .CreateCriteria<Member>()
      .Add(Expression.Eq("Active", true))
      .SetProjection(
        Projections.Count(Projections.Id())
      )
      .UniqueResult<int>();

    return count;
  }

  public Member FindMemberByName(string lastname, string firstname)
  {
    return
      (Member)
      Session.CreateCriteria(typeof(Member))
        .Add(Expression.Eq("Lastname", lastname))
        .Add(Expression.Eq("Firstname", firstname))
        .SetCacheable(true)
        .SetCacheMode(CacheMode.Normal)
        .UniqueResult()
      ;
  }

  public Member FindMemberByEmail(string email)
  {
    return
      (Member)
      Session.CreateCriteria(typeof(Member))
        .Add(Expression.Eq("Email", email))
        .SetCacheable(true)
        .SetCacheMode(CacheMode.Normal)
        .UniqueResult()
      ;
  }

  public IList<Member> FindMembersByEmailOrLogin(string email, string login)
  {
    return
      Session.CreateCriteria(typeof(Member))
        .CreateAlias("User", "u", JoinType.InnerJoin)
        .Add(Restrictions.Or(
          Restrictions.Eq("Email", email),
          Restrictions.Eq("u.Login", login)))
        .SetCacheable(true)
        .SetCacheMode(CacheMode.Normal)
        .List<Member>()
      ;
  }
}