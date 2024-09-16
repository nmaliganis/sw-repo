using sw.asset.model.Events;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sw.asset.contracts.ContractRepositories;

public interface IEventHistoryRepository : IRepository<EventHistory, Guid>
{
    QueryResult<EventHistory> FindAllPagedOf(int? pageNum, int? pageSize);
    QueryResult<EventHistory> FindAllByDeviceImeiPagedOf(string deviceImei, int? pageNum, int? pageSize);
    QueryResult<EventHistory> FindEventHistoryBetweenDatesPagedOf(DateTime startDate, DateTime endDate, int? pageNum, int? pageSize);

    Task CreateEventHistory(EventHistory eventHistoryToBeCreated, double eventValue);

    QueryResult<EventHistory> FindEventHistoryBetweenDatesAndDeviceImeiPagedOf(string deviceImei, DateTime startDate, DateTime endDate, int? pageNum, int? pageSize);
    IList<EventHistory> FindEventHistoryByDate(DateTime date);
    IList<EventHistory> FindByEventHistoryBySensorId(long sensorId);
    QueryResult<EventHistory> FindByNLastRecordsPagedOf(int n, int? pageNum, int? pageSize);
}// Class: IEventHistoryRepository