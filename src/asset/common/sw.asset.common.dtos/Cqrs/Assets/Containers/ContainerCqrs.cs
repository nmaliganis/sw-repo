using System.Collections.Generic;
using sw.asset.common.dtos.ResourceParameters.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.Assets.Containers;

// Queries
public record GetContainerByIdQuery(long Id) : IRequest<BusinessResult<ContainerUiModel>>;
public record GetContainerByImeiQuery(string Imei) : IRequest<BusinessResult<ContainerUiModel>>;
public class GetContainersQuery : GetContainersResourceParameters, IRequest<BusinessResult<PagedList<ContainerUiModel>>>
{
    public GetContainersQuery(GetContainersResourceParameters parameters) : base()
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

public class GetContainersByZoneIdQuery : GetContainersResourceParameters, IRequest<BusinessResult<PagedList<ContainerUiModel>>>
{
    public long ZoneId { get; }

    public GetContainersByZoneIdQuery(long zoneId, GetContainersResourceParameters parameters) : base()
    {
        ZoneId = zoneId;
        Filter = parameters.Filter;
        SearchQuery = parameters.SearchQuery;
        Fields = parameters.Fields;
        OrderBy = parameters.OrderBy;
        SortDirection = parameters.SortDirection;
        PageSize = parameters.PageSize;
        PageIndex = parameters.PageIndex;
    }
}

public class GetContainersByZonesQuery : IRequest<BusinessResult<List<ContainerUiModel>>>
{
	public List<long> Zones { get; }

	public GetContainersByZonesQuery(List<long> zones) : base()
	{
		this.Zones = zones;
	}
}

public record GetContainersCountTotalQuery() : IRequest<BusinessResult<ContainerCountUiModel>>;


public class GetContainersCountTotalInZonesQuery : IRequest<BusinessResult<ContainerCountUiModel>>
{
    public List<long> Zones { get; }

    public GetContainersCountTotalInZonesQuery(List<long> zones) : base()
    {
        this.Zones = zones;
    }
}

public class GetContainersByCriteriaInZonesQuery : IRequest<BusinessResult<List<ContainerUiModel>>>
{
    public List<long> Zones { get; }
    public string Criteria { get; }

    public GetContainersByCriteriaInZonesQuery(List<long> zones, string criteria) : base()
    {
        this.Zones = zones;
        this.Criteria = criteria;
    }
}

public class GetContainersByVolumeInZonesQuery : IRequest<BusinessResult<List<ContainerUiModel>>>
{
    public List<long> Zones { get; }
    public int LowerLevel { get; }
    public int UpperLevel { get; }

    public GetContainersByVolumeInZonesQuery(List<long> zones, int lowerLevel, int upperLevel) : base()
    {
        this.Zones = zones;
        this.LowerLevel = lowerLevel;
        this.UpperLevel = upperLevel;
    }
}

// Commands
public record CreateContainerCommand(long CreatedById, CreateContainerResourceParameters Parameters)
  : IRequest<BusinessResult<ContainerUiModel>>;

public record CreateContainerWithDeviceImeiCommand(long CreatedById, string DeviceImei, CreateContainerWithDeviceResourceParameters Parameters)
  : IRequest<BusinessResult<ContainerUiModel>>;

public record UpdateContainerCommand(long Id, long ModifiedById, UpdateContainerResourceParameters Parameters)
  : IRequest<BusinessResult<ContainerUiModel>>;

public record OnboardingContainerWithDeviceCommand(long ContainerId, long DeviceId, long ModifiedById, OnboardingContainerResourceParameters Parameters)
  : IRequest<BusinessResult<ContainerModificationUiModel>>;

public record OnboardingContainerWithDeviceByNameCommand(string ContainerName, string DeviceImei, long ModifiedById)
  : IRequest<BusinessResult<ContainerUiModel>>;

public record UpdateContainerWithLatLonCommand(long Id, double ModifiedLat, double ModifiedLon)
  : IRequest<BusinessResult<ContainerModificationUiModel>>;

public record UpdateContainerByNameWithLatLonCommand(string ContainerName, double ModifiedLat, double ModifiedLon)
	: IRequest<BusinessResult<ContainerModificationUiModel>>;

public record UpdateContainerWithLatLonByDeviceCommand(string DeviceImei, double ModifiedLat, double ModifiedLon)
	: IRequest<BusinessResult<ContainerModificationUiModel>>;


public record UpdateContainerMeasurementsCommand(string Type, string DeviceImei, ContainerModificationMeasurementsUiModel Parameters)
  : IRequest<BusinessResult<ContainerUiModel>>;

public record DeleteSoftContainerCommand(long Id, long DeletedBy, string DeletedReason)
  : IRequest<BusinessResult<ContainerDeletionUiModel>>;

public record DeleteHardContainerCommand(long Id)
  : IRequest<BusinessResult<ContainerDeletionUiModel>>;