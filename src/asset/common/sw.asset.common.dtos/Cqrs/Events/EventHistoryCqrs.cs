using sw.asset.common.dtos.ResourceParameters.Events;
using sw.asset.common.dtos.Vms.Events;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;
using System;

namespace sw.asset.common.dtos.Cqrs.Events;

// Queries
public record GetEventHistoryByIdQuery(long Id) : IRequest<BusinessResult<EventHistoryUiModel>>;

public class GetEventHistoryNLastRecordsQuery : GetEventHistoryResourceParameters, IRequest<BusinessResult<PagedList<EventHistoryUiModel>>>
{
    public int N{ get; set; }
    public GetEventHistoryNLastRecordsQuery(int n, GetEventHistoryResourceParameters parameters) : base()
    {
        N = n;
        Filter = parameters.Filter;
        SearchQuery = parameters.SearchQuery;
        Fields = parameters.Fields;
        OrderBy = parameters.OrderBy;
        SortDirection = parameters.SortDirection;
        PageSize = parameters.PageSize;
        PageIndex = parameters.PageIndex;
    }
}

public class GetEventHistoryBetweenDatesForContainerQuery : GetEventHistoryResourceParameters, IRequest<BusinessResult<PagedList<EventHistoryUiModel>>>
{
    public long ContainerId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public GetEventHistoryBetweenDatesForContainerQuery(long containerId, DateTime start, DateTime end, GetEventHistoryResourceParameters parameters) : base()
    {
        Start = start;
        End = end;
        ContainerId = containerId;
        Filter = parameters.Filter;
        SearchQuery = parameters.SearchQuery;
        Fields = parameters.Fields;
        OrderBy = parameters.OrderBy;
        SortDirection = parameters.SortDirection;
        PageSize = parameters.PageSize;
        PageIndex = parameters.PageIndex;
    }
}
public class GetEventHistoryByContainerIdQuery : GetEventHistoryResourceParameters, IRequest<BusinessResult<PagedList<EventHistoryUiModel>>>
{
    public long ContainerId { get; set; }
    public GetEventHistoryByContainerIdQuery(long containerId, GetEventHistoryResourceParameters parameters) : base()
    {
        ContainerId = containerId;
        Filter = parameters.Filter;
        SearchQuery = parameters.SearchQuery;
        Fields = parameters.Fields;
        OrderBy = parameters.OrderBy;
        SortDirection = parameters.SortDirection;
        PageSize = parameters.PageSize;
        PageIndex = parameters.PageIndex;
    }
}
public class GetEventHistoryByDeviceQuery : GetEventHistoryResourceParameters, IRequest<BusinessResult<PagedList<EventHistoryUiModel>>>
{
    public string DeviceImei { get; set; }
    public GetEventHistoryByDeviceQuery(string deviceImei, GetEventHistoryResourceParameters parameters) : base()
    {
        DeviceImei = deviceImei;
        Filter = parameters.Filter;
        SearchQuery = parameters.SearchQuery;
        Fields = parameters.Fields;
        OrderBy = parameters.OrderBy;
        SortDirection = parameters.SortDirection;
        PageSize = parameters.PageSize;
        PageIndex = parameters.PageIndex;
    }
}
public class GetEventHistoryBetweenDatesQuery : GetEventHistoryResourceParameters, IRequest<BusinessResult<PagedList<EventHistoryUiModel>>>
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public GetEventHistoryBetweenDatesQuery(DateTime start, DateTime end, GetEventHistoryResourceParameters parameters) : base()
    {
        Start = start;
        End = end;
        Filter = parameters.Filter;
        SearchQuery = parameters.SearchQuery;
        Fields = parameters.Fields;
        OrderBy = parameters.OrderBy;
        SortDirection = parameters.SortDirection;
        PageSize = parameters.PageSize;
        PageIndex = parameters.PageIndex;
    }
}
public class GetEventHistoryBetweenDatesForDeviceQuery : GetEventHistoryResourceParameters, IRequest<BusinessResult<PagedList<EventHistoryUiModel>>>
{
    public string DeviceImei { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public GetEventHistoryBetweenDatesForDeviceQuery(string deviceImei, DateTime start, DateTime end, GetEventHistoryResourceParameters parameters) : base()
    {
        DeviceImei = deviceImei;
        Start = start;
        End = end;
        Filter = parameters.Filter;
        SearchQuery = parameters.SearchQuery;
        Fields = parameters.Fields;
        OrderBy = parameters.OrderBy;
        SortDirection = parameters.SortDirection;
        PageSize = parameters.PageSize;
        PageIndex = parameters.PageIndex;

    }
}

public class GetEventHistoryQuery : GetEventHistoryResourceParameters, IRequest<BusinessResult<PagedList<EventHistoryUiModel>>>
{
    public GetEventHistoryQuery(GetEventHistoryResourceParameters parameters) : base()
    {
        Filter = parameters.Filter;
        SearchQuery = parameters.SearchQuery;
        Fields = parameters.Fields;
        OrderBy = parameters.OrderBy;
        SortDirection = parameters.SortDirection;
        PageSize = parameters.PageSize;
        PageIndex = parameters.PageIndex;
    }
}

// Commands
public record CreateEventHistoryCommand(int Type, string DeviceImei, CreateEventHistoryResourceParameters Parameters)
  : IRequest<BusinessResult<EventHistoryUiModel>>;

public record UpdateEventHistoryCommand(long ModifiedById, Guid Id, UpdateEventHistoryResourceParameters Parameters)
  : IRequest<BusinessResult<EventHistoryModificationUiModel>>;

public record DeleteHardEventHistoryCommand(Guid Id)
  : IRequest<BusinessResult<EventHistoryDeletionUiModel>>;