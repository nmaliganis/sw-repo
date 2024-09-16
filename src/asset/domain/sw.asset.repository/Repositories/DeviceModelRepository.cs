using sw.asset.contracts.ContractRepositories;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System.Linq;
using sw.asset.model.Devices;
using NHibernate;
using NHibernate.Criterion;

namespace sw.asset.repository.Repositories;

public class DeviceModelRepository : RepositoryBase<DeviceModel, long>, IDeviceModelRepository
{
    public DeviceModelRepository(ISession session)
        : base(session)
    {
    }

    public QueryResult<DeviceModel> FindAllActivePagedOf(int? pageNum, int? pageSize)
    {
        var query = Session.QueryOver<DeviceModel>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<DeviceModel>(query?
                .Where(dm => dm.Active == true)
                .List().AsQueryable());
        }

        return new QueryResult<DeviceModel>(query
                    .Where(dm => dm.Active == true)
                    .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                    .Take((int)pageSize).List().AsQueryable(),
                query.ToRowCountQuery().RowCount(),
                (int)pageSize)
            ;
    }

    public DeviceModel FindActiveById(long id)
    {
        DeviceModel dm = null;

        return this.Session.QueryOver<DeviceModel>(() => dm)
            .Where(() => dm.Active == true)
            .And(() => dm.Id == id)
            .Cacheable()
            .CacheMode(CacheMode.Normal)
            .SetFlushMode(FlushMode.Manual)
            .SingleOrDefault();
    }
    public DeviceModel FindByName(string name)
    {
        return (DeviceModel)
            Session.CreateCriteria(typeof(DeviceModel))
                .Add(Restrictions.Eq("Name", name))
                .UniqueResult()
            ;
    }
}// Class: DeviceModelRepository
