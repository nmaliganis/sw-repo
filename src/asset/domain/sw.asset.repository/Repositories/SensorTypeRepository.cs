using sw.asset.contracts.ContractRepositories;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using System.Linq;
using NHibernate.Criterion;
using System.Xml.Linq;
using sw.asset.model.SensorTypes;

namespace sw.asset.repository.Repositories;

public class SensorTypeRepository : RepositoryBase<SensorType, long>, ISensorTypeRepository
{
  public SensorTypeRepository(ISession session)
    : base(session)
  {
  }

  public QueryResult<SensorType> FindAllActivePagedOf(int? pageNum, int? pageSize)
  {
    var query = Session.QueryOver<SensorType>();

    if (pageNum == -1 & pageSize == -1)
    {
      return new QueryResult<SensorType>(query?
        .Where(snt => snt.Active == true)
        .List().AsQueryable());
    }

    return new QueryResult<SensorType>(query
          .Where(snt => snt.Active == true)
          .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
          .Take((int)pageSize).List().AsQueryable(),
        query.ToRowCountQuery().RowCount(),
        (int)pageSize)
      ;
  }

  public SensorType FindActiveById(long id)
  {
    SensorType senT = null;

    return this.Session.QueryOver<SensorType>(() => senT)
      .Where(() => senT.Active == true)
      .And(() => senT.Id == id)
      .Cacheable()
      .CacheMode(CacheMode.Normal)
      .SetFlushMode(FlushMode.Manual)
      .SingleOrDefault();
  }

  public SensorType FindSensorTypeByName(string name)
  {
    return
      (SensorType)
      Session.CreateCriteria(typeof(SensorType))
        .Add(Expression.Eq("Name", name))
        .Add(Expression.Eq("Active", true))
        .SetCacheable(true)
        .SetCacheMode(CacheMode.Normal)
        .UniqueResult()
      ;
  }

    public SensorType FindBySensorTypeIndex(int sensorTypeIndex)
    {
        return
          (SensorType)
          Session.CreateCriteria(typeof(SensorType))
            .Add(Expression.Eq("SensorTypeIndex", sensorTypeIndex))
            .Add(Expression.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .UniqueResult()
          ;
    }
}// Class: SensorTypeRepository

