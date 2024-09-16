using System.Collections.Generic;
using System.Linq;
using sw.asset.contracts.ContractRepositories;
using sw.asset.model.Devices.Simcards;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;

namespace sw.asset.repository.Repositories;

public class SimcardRepository : RepositoryBase<Simcard, long>, ISimcardRepository
{
  public SimcardRepository(ISession session)
    : base(session)
  {
  }

  public IList<Simcard> FindAllActiveSimcards()
  {
    return
      Session.CreateCriteria(typeof(Simcard))
        .Add(Expression.Eq("Active", true))
        .SetCacheable(true)
        .List<Simcard>()
      ;
  }

  public Simcard FindOneActiveById(long id)
  {
    return (Simcard)
      Session.CreateCriteria(typeof(Simcard))
        .Add(Restrictions.Eq("Id", id))
        .Add(Restrictions.Eq("Active", true))
        .SetCacheable(true)
        .UniqueResult()
      ;
  }

  public Simcard FindOneByIccid(string iccid)
  {
    return (Simcard)
      Session.CreateCriteria(typeof(Simcard))
        .Add(Restrictions.Eq("Iccid", iccid))
        .SetCacheable(true)
        .UniqueResult()
      ;
  }

  public Simcard FindOneByImsi(string imsi)
  {
    return (Simcard)
      Session.CreateCriteria(typeof(Simcard))
        .Add(Restrictions.Eq("Imsi", imsi))
        .SetCacheable(true)
        .UniqueResult()
      ;
  }

  public Simcard FindOneByIccidAndImsi(string iccid, string imsi)
  {
    return (Simcard)
      Session.CreateCriteria(typeof(Simcard))
        .Add(Restrictions.Eq("Iccid", iccid))
        .Add(Restrictions.Eq("Imsi", imsi))
        .SetCacheable(true)
        .UniqueResult()
      ;
  }

  public Simcard FindOneByIccidOrImsiOrNumber(string iccid, string imsi, string number)
  {
    return (Simcard)
      Session.CreateCriteria(typeof(Simcard))
        .Add(
          Expression.Disjunction()
            .Add(Restrictions.Eq("Imsi", imsi))
            .Add(Restrictions.Eq("Iccid", iccid))
            .Add(Restrictions.Eq("Number",number))
        )
        .SetCacheable(true)
        .UniqueResult()
      ;
  }

  public Simcard FindOneByNumber(string number)
  {
    return (Simcard)
      Session.CreateCriteria(typeof(Simcard))
        .Add(Restrictions.Eq("Number", number))
        .UniqueResult()
      ;
  }

  public QueryResult<Simcard> FindAllActivePagedOf(int? pageNum, int? pageSize)
  {
    var query = Session.QueryOver<Simcard>();

    if (pageNum == -1 & pageSize == -1)
    {
      return new QueryResult<Simcard>(query?.List().AsQueryable());
    }

    return new QueryResult<Simcard>(query
          .Where(mc => mc.Active == true)
          .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
          .Take((int) pageSize).List().AsQueryable(),
        query.ToRowCountQuery().RowCount(),
        (int) pageSize)
      ;
  }
  public QueryResult<Simcard> FindAllSimcardsPagedOf(int? pageNum, int? pageSize)
  {
    var query = Session.QueryOver<Simcard>();

    if (pageNum == -1 & pageSize == -1)
    {
      return new QueryResult<Simcard>(query?.List().AsQueryable());
    }

    return new QueryResult<Simcard>(query
          .Where(mc => mc.Active == true)
          .Skip(ResultsPagingUtility.CalculateStartIndex((int) pageNum, (int) pageSize))
          .Take((int) pageSize).List().AsQueryable(),
        query.ToRowCountQuery().RowCount(),
        (int) pageSize)
      ;
  }
}