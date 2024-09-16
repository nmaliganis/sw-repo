using sw.asset.common.dtos.ResourceParameters.Assets.Vehicles;
using sw.asset.common.dtos.Vms.Assets.Vehicles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.Assets.Vehicles
{
    // Queries
    public record GetVehicleByIdQuery(long Id) : IRequest<BusinessResult<VehicleUiModel>>;

    public class GetVehiclesQuery : GetVehiclesResourceParameters, IRequest<BusinessResult<PagedList<VehicleUiModel>>>
    {
        public GetVehiclesQuery(GetVehiclesResourceParameters parameters) : base()
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
    public record CreateVehicleCommand(long CreatedById, CreateVehicleResourceParameters parameters)
        : IRequest<BusinessResult<VehicleCreationUiModel>>;

    public record UpdateVehicleCommand(long Id, long ModifiedById, UpdateVehicleResourceParameters parameters)
        : IRequest<BusinessResult<VehicleModificationUiModel>>;

    public record DeleteSoftVehicleCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<VehicleDeletionUiModel>>;

    public record DeleteHardVehicleCommand(long Id)
        : IRequest<BusinessResult<VehicleDeletionUiModel>>;
}
