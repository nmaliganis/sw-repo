using sw.asset.contracts.ContractRepositories;
using sw.asset.model.Devices;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using System.Linq;

namespace sw.asset.repository.Repositories;

public class DeviceRepository : RepositoryBase<Device, long>, IDeviceRepository
{

    public DeviceRepository(ISession session)
      : base(session)
    {
    }

    public QueryResult<Device> FindAllActivePagedOf(int? pageNum, int? pageSize)
    {
        var query = Session.QueryOver<Device>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<Device>(query?
              .Where(r => r.Active == true)
              .List().AsQueryable());
        }

        return new QueryResult<Device>(query
              .Where(r => r.Active == true)
              .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
              .Take((int)pageSize).List().AsQueryable(),
            query.ToRowCountQuery().RowCount(),
            (int)pageSize)
          ;
    }

    public int FindCountAllActive()
    {
        int count;

        count = Session
          .CreateCriteria<Device>()
          .Add(Expression.Eq("Active", true))
          .SetProjection(
            Projections.Count(Projections.Id())
          )
          .UniqueResult<int>();

        return count;
    }

    public Device FindOneByImei(string imei)
    {
        return (Device)
          Session.CreateCriteria(typeof(Device))
            .Add(Restrictions.Eq("Imei", imei))
            .Add(Restrictions.Eq("Active", true))
            .UniqueResult()
          ;
    }

    public Device FindOneBySerialNumber(string serialNumber)
    {
        return (Device)
          Session.CreateCriteria(typeof(Device))
            .Add(Restrictions.Eq("SerialNumber", serialNumber))
            .Add(Restrictions.Eq("Active", true))
            .UniqueResult()
          ;
    }

    public Device FindActiveById(long id)
    {
        return
          (Device)
          Session.CreateCriteria(typeof(Device))
            .Add(Expression.Eq("Id", id))
            .Add(Expression.Eq("Active", true))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .UniqueResult()
          ;
    }
}// Class: DeviceRepository