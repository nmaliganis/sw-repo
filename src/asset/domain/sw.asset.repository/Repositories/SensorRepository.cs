using sw.asset.contracts.ContractRepositories;
using sw.asset.model.Sensors;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System.Collections.Generic;
using System.Linq;

namespace sw.asset.repository.Repositories;

public class SensorRepository : RepositoryBase<Sensor, long>, ISensorRepository
{
    public SensorRepository(ISession session)
        : base(session)
    {
    }

    public QueryResult<Sensor> FindAllActivePagedOf(int? pageNum, int? pageSize)
    {
        var query = Session.QueryOver<Sensor>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<Sensor>(query?
              .Where(r => r.Active == true)
              .List().AsQueryable());
        }

        return new QueryResult<Sensor>(query
              .Where(r => r.Active == true)
              .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
              .Take((int)pageSize).List().AsQueryable(),
            query.ToRowCountQuery().RowCount(),
            (int)pageSize)
          ;
    }

    public Sensor FindActiveById(long id)
    {
        return
            (Sensor)
            Session.CreateCriteria(typeof(Sensor))
              .Add(Restrictions.Eq("Id", id))
              .Add(Restrictions.Eq("Active", true))
              .CreateAlias("Device", "d", JoinType.InnerJoin)
              .Add(Restrictions.Eq("d.Active", true))
              .CreateAlias("SensorType", "s", JoinType.InnerJoin)
              .Add(Restrictions.Eq("s.Active", true))
              .CreateAlias("Asset", "a", JoinType.InnerJoin)
              .Add(Restrictions.Eq("a.Active", true))
              .SetCacheable(true)
              .SetCacheMode(CacheMode.Normal)
              .SetFlushMode(FlushMode.Never)
              .UniqueResult()
            ;
    }

    public Sensor FindOneByName(string name)
    {
        return (Sensor)
          Session.CreateCriteria(typeof(Sensor))
            .Add(Restrictions.Eq("Name", name))
            .Add(Restrictions.Eq("Active", true))
            .UniqueResult()
          ;
    }

    public Sensor FindOneByNameAndDeviceId(string name, long deviceId)
    {
        return
          (Sensor)
          Session.CreateCriteria(typeof(Sensor))
            .CreateAlias("Device", "d", JoinType.InnerJoin)
            .Add(Restrictions.Eq("d.Id", deviceId))
            .Add(Restrictions.Eq("d.Active", true))
            .Add(Restrictions.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .SetFlushMode(FlushMode.Never)
            .UniqueResult()
          ;
    }

    public Sensor FindOneByTypeAndDeviceImei(string sensorTypeName, string deviceImei)
    {
        return
          (Sensor)
          Session.CreateCriteria(typeof(Sensor))
            .CreateAlias("Device", "d", JoinType.InnerJoin)
            .CreateAlias("SensorType", "st", JoinType.InnerJoin)
            .Add(Expression.IsNotNull("Asset"))
            .Add(Restrictions.Eq("d.Imei", deviceImei))
            .Add(Restrictions.Eq("d.Active", true))
            .Add(Expression.Like("st.Name", $"{sensorTypeName}%"))
            .Add(Restrictions.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .SetFlushMode(FlushMode.Never)
            .UniqueResult()
          ;
    }

    public IList<Sensor> FindByTypeAndDeviceImei(string sensorTypeName, string deviceImei)
    {
        return
          Session.CreateCriteria(typeof(Sensor))
            .CreateAlias("Device", "d", JoinType.InnerJoin)
            .CreateAlias("SensorType", "st", JoinType.InnerJoin)
            .Add(Expression.IsNotNull("Asset"))
            .Add(Restrictions.Eq("d.Imei", deviceImei))
            .Add(Restrictions.Eq("d.Active", true))
            .Add(Expression.Like("st.Name", $"{sensorTypeName}%"))
            .Add(Restrictions.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .SetFlushMode(FlushMode.Never)
            .List<Sensor>()
          ;
    }

    public IList<Sensor> FindByDeviceImeiAndSensorTypeIndex(string deviceImei, int sensorTypeIndex)
    {
	    return
		    Session.CreateCriteria(typeof(Sensor))
			    .CreateAlias("Device", "d", JoinType.InnerJoin)
			    .CreateAlias("SensorType", "st", JoinType.InnerJoin)
			    .Add(Expression.IsNotNull("Asset"))
			    .Add(Restrictions.Eq("d.Imei", deviceImei))
			    .Add(Restrictions.Eq("d.Active", true))
			    .Add(Expression.Eq("st.SensorTypeIndex", sensorTypeIndex))
			    .Add(Restrictions.Eq("Active", true))
			    .SetCacheable(true)
			    .SetCacheMode(CacheMode.Normal)
			    .SetFlushMode(FlushMode.Never)
			    .List<Sensor>()
		    ;
    }

	public IList<Sensor> FindByDeviceImei(string deviceImei)
    {
        return
            Session.CreateCriteria(typeof(Sensor))
                .CreateAlias("Device", "d", JoinType.InnerJoin)
                .Add(Expression.IsNotNull("Asset"))
                .Add(Restrictions.Eq("d.Imei", deviceImei))
                .Add(Restrictions.Eq("d.Active", true))
                .Add(Restrictions.Eq("Active", true))
                .SetCacheable(true)
                .SetCacheMode(CacheMode.Normal)
                .SetFlushMode(FlushMode.Never)
                .List<Sensor>()
            ;
    }

    public IList<Sensor> FindByAllByContainerId(long containerId)
    {
        return
          Session.CreateCriteria(typeof(Sensor))
            .CreateAlias("Asset", "a", JoinType.InnerJoin)
            .Add(Expression.IsNotNull("Asset"))
            .Add(Restrictions.Eq("a.Id", containerId))
            .Add(Restrictions.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .SetFlushMode(FlushMode.Never)
            .List<Sensor>()
          ;
    }
}// Class: SensorRepository