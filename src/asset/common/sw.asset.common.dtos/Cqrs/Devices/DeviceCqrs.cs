using sw.asset.common.dtos.ResourceParameters.Devices;
using sw.asset.common.dtos.Vms.Devices;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.Devices;

// Queries
public record GetDeviceByIdQuery(long Id) : IRequest<BusinessResult<DeviceUiModel>>;

public class GetDevicesQuery : GetDevicesResourceParameters, IRequest<BusinessResult<PagedList<DeviceUiModel>>>
{
    public GetDevicesQuery(GetDevicesResourceParameters parameters) : base()
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
public record CreateDeviceCommand(long CreatedById, CreateDeviceResourceParameters Parameters)
    : IRequest<BusinessResult<DeviceUiModel>>;

public record UpdateDeviceCommand(long Id, long ModifiedById, UpdateDeviceResourceParameters Parameters)
: IRequest<BusinessResult<DeviceModificationUiModel>>;

public record DeleteSoftDeviceCommand(long Id, long DeletedBy, string DeletedReason)
    : IRequest<BusinessResult<DeviceDeletionUiModel>>;

public record DeleteHardDeviceCommand(long Id)
    : IRequest<BusinessResult<DeviceDeletionUiModel>>;
