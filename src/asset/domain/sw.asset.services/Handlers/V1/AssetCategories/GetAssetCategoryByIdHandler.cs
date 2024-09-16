using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.AssetCategories;

internal class GetAssetCategoryByIdHandler :
    IRequestHandler<GetAssetCategoryByIdQuery, BusinessResult<AssetCategoryUiModel>>
{
    private readonly IGetAssetCategoryByIdProcessor _processor;

    public GetAssetCategoryByIdHandler(IGetAssetCategoryByIdProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<AssetCategoryUiModel>> Handle(GetAssetCategoryByIdQuery qry, CancellationToken cancellationToken)
    {
        return await _processor.GetAssetCategoryByIdAsync(qry.Id);
    }
}
