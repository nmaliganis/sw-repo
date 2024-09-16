using sw.asset.common.dtos.ResourceParameters.SensorTypes;
using sw.asset.common.dtos.Vms.SensorTypes;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.SensorTypes;

// Queries
public record GetSensorTypeByIdQuery(long Id) : IRequest<BusinessResult<SensorTypeUiModel>>;

public class GetSensorTypesQuery : GetSensorTypesResourceParameters, IRequest<BusinessResult<PagedList<SensorTypeUiModel>>>
{
  public GetSensorTypesQuery(GetSensorTypesResourceParameters parameters) : base()
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
public record CreateSensorTypeCommand(long CreatedById, CreateSensorTypeResourceParameters Parameters)
  : IRequest<BusinessResult<SensorTypeUiModel>>;

public record UpdateSensorTypeCommand(long ModifiedById, long Id, UpdateSensorTypeResourceParameters Parameters)
  : IRequest<BusinessResult<SensorTypeModificationUiModel>>;

public record DeleteSoftSensorTypeCommand(long Id, long DeletedBy, string DeletedReason)
  : IRequest<BusinessResult<SensorTypeDeletionUiModel>>;

public record DeleteHardSensorTypeCommand(long Id)
  : IRequest<BusinessResult<SensorTypeDeletionUiModel>>;