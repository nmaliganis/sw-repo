using sw.asset.contracts.ContractRepositories;
using sw.asset.model.Events;
using sw.asset.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Exceptions.Repositories.NHibernate;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace sw.asset.repository.Repositories;

public class EventHistoryRepository : RepositoryBase<EventHistory, Guid>, IEventHistoryRepository
{
    public EventHistoryRepository(ISession session)
      : base(session)
    {
    }

    public QueryResult<EventHistory> FindAllPagedOf(int? pageNum, int? pageSize)
    {
        var query = Session.QueryOver<EventHistory>();

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<EventHistory>(query?
              .List().AsQueryable());
        }

        return new QueryResult<EventHistory>(query
              .Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
              .Take((int)pageSize).List().AsQueryable(),
            query.ToRowCountQuery().RowCount(),
            (int)pageSize)
          ;
    }

    public QueryResult<EventHistory> FindAllByDeviceImeiPagedOf(string deviceImei, int? pageNum, int? pageSize)
    {
        var criteriaBuilder = this.Session.CreateCriteria(typeof(EventHistory));
        criteriaBuilder.CreateAlias("Sensor", "s", JoinType.InnerJoin);
        criteriaBuilder.CreateAlias("s.Device", "d", JoinType.InnerJoin);

        var conjunction = Restrictions.Conjunction();

        conjunction.Add(Expression.Eq("d.Imei", deviceImei));

        criteriaBuilder.Add(conjunction);
        criteriaBuilder.AddOrder(Order.Asc("Recorded"));
        criteriaBuilder.SetCacheable(true);
        criteriaBuilder.SetCacheMode(CacheMode.Normal);

        //Added for Distinct
        //criteriaBuilder.SetProjection(
        //    Projections.Distinct(Projections.ProjectionList()
        //        .Add(Projections.Alias(Projections.Property("Recorded"), "Recorded"))));

        //criteriaBuilder.SetResultTransformer(
        //    new NHibernate.Transform.AliasToBeanResultTransformer(typeof(EventHistory)));

        //criteriaBuilder.SetResultTransformer(NHibernate.Transform.Transformers.DistinctRootEntity);

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<EventHistory>(criteriaBuilder.List<EventHistory>().AsQueryable()); ;
        }

        return new QueryResult<EventHistory>(criteriaBuilder
            .SetFirstResult(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
            .SetMaxResults((int)pageSize)
            .List<EventHistory>().AsQueryable(),
          criteriaBuilder.SetProjection(Projections.RowCount()).UniqueResult<int>(),
          (int)pageSize);
    }

    public QueryResult<EventHistory> FindEventHistoryBetweenDatesPagedOf(DateTime startDate, DateTime endDate, int? pageNum, int? pageSize)
    {
        var criteriaBuilder = this.Session.CreateCriteria(typeof(EventHistory));
        var conjunction = Restrictions.Conjunction();

        conjunction.Add(
          Expression.Conjunction()
            .Add(Restrictions.Ge("Recorded", startDate))
            .Add(Restrictions.Lt("Recorded", endDate.Date.AddDays(1)))
        );

        criteriaBuilder.Add(conjunction);
        criteriaBuilder.AddOrder(Order.Asc("Recorded"));
        criteriaBuilder.SetCacheable(true);
        criteriaBuilder.SetCacheMode(CacheMode.Normal);

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<EventHistory>(criteriaBuilder.List<EventHistory>().AsQueryable()); ;
        }

        return new QueryResult<EventHistory>(criteriaBuilder
            .SetFirstResult(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
            .SetMaxResults((int)pageSize)
            .List<EventHistory>().AsQueryable(),
          criteriaBuilder.SetProjection(Projections.RowCount()).UniqueResult<int>(),
          (int)pageSize);
        
    }


    public async Task CreateEventHistory(EventHistory eventHistoryToBeCreated, double eventValue)
    {
        using ITransaction transaction = Session?.BeginTransaction();
        try
        {
            if (!Session.GetCurrentTransaction()!.IsActive)
                return;
            if (Session.IsNull())
                return;

            string queryStr =
                $"INSERT INTO \"EventHistory\" " +
                $"(eventvaluejson, " +
                $"recorded, " +
                $"received, " +
                $"eventvalue, " +
                $"\"sensorId\", " +
                $"id) " +
                $"VALUES ('{{\"Params\": " +
                $@"{eventHistoryToBeCreated.EventValue.Params}" +
                $"}}', " +
                $"'{eventHistoryToBeCreated.Recorded:yyyy'-'MM'-'dd'T'HH':'mm':'ss}', " +
                $"'{eventHistoryToBeCreated.Received:yyyy'-'MM'-'dd'T'HH':'mm':'ss}', " +
                $"'{eventValue}', " +
                $"{eventHistoryToBeCreated.Sensor.Id}, " +
                $"'{Guid.NewGuid()}');"
                ;

            var query = Session!.CreateSQLQuery(queryStr);
            var result = await query.ExecuteUpdateAsync();

            transaction?.CommitAsync();
            await Session.FlushAsync();
        }
        catch (Exception ex)
        {
            transaction?.RollbackAsync();
            throw new NHibernateSessionTransactionFailedException(ex.Message);
        }
    }

    public QueryResult<EventHistory> FindEventHistoryBetweenDatesAndDeviceImeiPagedOf(string deviceImei, DateTime startDate, DateTime endDate, int? pageNum, int? pageSize)
    {
        var criteriaBuilder = this.Session.CreateCriteria(typeof(EventHistory));
        criteriaBuilder.CreateAlias("Sensor", "s", JoinType.InnerJoin);
        criteriaBuilder.CreateAlias("s.Device", "d", JoinType.InnerJoin);

        var conjunction = Restrictions.Conjunction();
        
        conjunction.Add(Expression.Eq("d.Imei", deviceImei));

        //Added for Distinct
        //criteriaBuilder.SetProjection(
        //    Projections.Distinct(Projections.ProjectionList()
        //        .Add(Projections.Alias(Projections.Property("Recorded"), "Recorded"))));

        //criteriaBuilder.SetResultTransformer(
        //    new NHibernate.Transform.AliasToBeanResultTransformer(typeof(EventHistory)));

        //criteriaBuilder.SetResultTransformer(NHibernate.Transform.Transformers.DistinctRootEntity);

        criteriaBuilder.Add(conjunction);
        criteriaBuilder.AddOrder(Order.Asc("Recorded"));
        criteriaBuilder.SetCacheable(true);
        criteriaBuilder.SetCacheMode(CacheMode.Normal);

        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<EventHistory>(criteriaBuilder.List<EventHistory>().AsQueryable()); ;
        }

        return new QueryResult<EventHistory>(criteriaBuilder
            .SetFirstResult(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
            .SetMaxResults((int)pageSize)
            .List<EventHistory>().AsQueryable(),
          criteriaBuilder.SetProjection(Projections.RowCount()).UniqueResult<int>(),
          (int)pageSize);
        
    }

    public IList<EventHistory> FindEventHistoryByDate(DateTime date)
    {
        return
          Session.CreateCriteria(typeof(EventHistory))
            .Add(Expression.Eq("Recorded", date))
            .SetCacheable(true)
            .SetCacheMode(CacheMode.Normal)
            .List<EventHistory>()
          ;
    }

    public IList<EventHistory> FindByEventHistoryBySensorId(long sensorId)
    {
        EventHistory ev = null;
        return this.Session.QueryOver<EventHistory>(() => ev)
          .Where(() => ev.Sensor.Id == sensorId)
          .Cacheable()
          .CacheMode(CacheMode.Normal)
          .SetFlushMode(FlushMode.Manual)
          .List<EventHistory>()
          ;
    }

    public QueryResult<EventHistory> FindByNLastRecordsPagedOf(int n, int? pageNum, int? pageSize)
    {
        var criteria = Session.CreateCriteria(typeof(EventHistory))
           .AddOrder(Order.Desc("Recorded"))
           .SetMaxResults(n)
         ;
        if (pageNum == -1 & pageSize == -1)
        {
            return new QueryResult<EventHistory>(criteria.List<EventHistory>().AsQueryable()); ;
        }
        return new QueryResult<EventHistory>(criteria
            .SetFirstResult(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
            .SetMaxResults((int)pageSize)
            .List<EventHistory>().AsQueryable(),
          criteria.SetProjection(Projections.RowCount()).UniqueResult<int>(),
          (int)pageSize);
        ;
    }
}// Class: EventHistoryRepository

