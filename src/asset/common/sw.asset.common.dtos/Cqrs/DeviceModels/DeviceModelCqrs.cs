using sw.asset.common.dtos.ResourceParameters.DeviceModels;
using sw.asset.common.dtos.Vms.DeviceModels;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.DeviceModels
{
    // Queries
    public record GetDeviceModelByIdQuery(long Id) : IRequest<BusinessResult<DeviceModelUiModel>>;

    public class GetDeviceModelsQuery : GetDeviceModelsResourceParameters, IRequest<BusinessResult<PagedList<DeviceModelUiModel>>>
    {
        public GetDeviceModelsQuery(GetDeviceModelsResourceParameters parameters) : base()
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
    public record CreateDeviceModelCommand(long CreatedById, string Name, string CodeErp, string CodeName, bool Enabled)
        : IRequest<BusinessResult<DeviceModelCreationUiModel>>;

    public record UpdateDeviceModelCommand(long ModifiedById, long Id, string Name, string CodeErp, string CodeName, bool Enabled)
        : IRequest<BusinessResult<DeviceModelModificationUiModel>>;

    public record DeleteSoftDeviceModelCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<DeviceModelDeletionUiModel>>;

    public record DeleteHardDeviceModelCommand(long Id)
        : IRequest<BusinessResult<DeviceModelDeletionUiModel>>;
}
