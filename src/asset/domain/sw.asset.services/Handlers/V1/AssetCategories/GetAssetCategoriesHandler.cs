using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.AssetCategories;

internal class GetAssetCategoriesHandler :
    IRequestHandler<GetAssetCategoriesQuery, BusinessResult<PagedList<AssetCategoryUiModel>>>
{
    private readonly IGetAssetCategoriesProcessor _processor;

    public GetAssetCategoriesHandler(IGetAssetCategoriesProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<PagedList<AssetCategoryUiModel>>> Handle(GetAssetCategoriesQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetAssetCategoriesAsync(qry);
    }
}