using sw.asset.common.dtos.ResourceParameters.Sensor;
using sw.asset.common.dtos.ResourceParameters.Sensors;
using sw.asset.common.dtos.Vms.Sensors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.Sensor
{
    // Queries
    public record GetSensorByIdQuery(long Id) : IRequest<BusinessResult<SensorUiModel>>;

    public class GetSensorsQuery : GetSensorsResourceParameters, IRequest<BusinessResult<PagedList<SensorUiModel>>>
    {
        public GetSensorsQuery(GetSensorsResourceParameters parameters) : base()
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
    public record CreateSensorCommand(long CreatedById, CreateSensorResourceParameters Parameters)
        : IRequest<BusinessResult<SensorUiModel>>;

    public record CreateSensorByDeviceImeiCommand(long CreatedById, string deviceImei, CreateSensorByImeiResourceParameters Parameters)
        : IRequest<BusinessResult<SensorUiModel>>;

    public record UpdateSensorCommand(long ModifiedById, long Id, UpdateSensorResourceParameters Parameters)
        : IRequest<BusinessResult<SensorModificationUiModel>>;

    public record DeleteSoftSensorCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<SensorDeletionUiModel>>;

    public record DeleteHardSensorCommand(long Id)
        : IRequest<BusinessResult<SensorDeletionUiModel>>;
}
