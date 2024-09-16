using sw.asset.common.dtos.ResourceParameters.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.AssetCategories
{
    // Queries
    public record GetAssetCategoryByIdQuery(long Id) : IRequest<BusinessResult<AssetCategoryUiModel>>;

    public class GetAssetCategoriesQuery : GetAssetCategoriesResourceParameters, IRequest<BusinessResult<PagedList<AssetCategoryUiModel>>>
    {
        public GetAssetCategoriesQuery(GetAssetCategoriesResourceParameters parameters) : base()
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
    public record CreateAssetCategoryCommand(long CreatedById, string Name, string CodeErp, string Params)
        : IRequest<BusinessResult<AssetCategoryCreationUiModel>>;

    public record UpdateAssetCategoryCommand(long ModifiedById, long Id, string Name, string CodeErp, string Params)
        : IRequest<BusinessResult<AssetCategoryModificationUiModel>>;

    public record DeleteSoftAssetCategoryCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<AssetCategoryDeletionUiModel>>;

    public record DeleteHardAssetCategoryCommand(long Id)
        : IRequest<BusinessResult<AssetCategoryDeletionUiModel>>;
}
