using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.AssetCategories;

internal class DeleteSoftAssetCategoryHandler :
    IRequestHandler<DeleteSoftAssetCategoryCommand, BusinessResult<AssetCategoryDeletionUiModel>>
{
    private readonly IDeleteSoftAssetCategoryProcessor _processor;

    public DeleteSoftAssetCategoryHandler(IDeleteSoftAssetCategoryProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<AssetCategoryDeletionUiModel>> Handle(DeleteSoftAssetCategoryCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteSoftAssetCategoryAsync(deleteCommand);
    }
}