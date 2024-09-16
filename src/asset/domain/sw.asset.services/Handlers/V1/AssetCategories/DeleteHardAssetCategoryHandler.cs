using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.AssetCategories;

internal class DeleteHardAssetCategoryHandler :
    IRequestHandler<DeleteHardAssetCategoryCommand, BusinessResult<AssetCategoryDeletionUiModel>>
{
    private readonly IDeleteHardAssetCategoryProcessor _processor;

    public DeleteHardAssetCategoryHandler(IDeleteHardAssetCategoryProcessor processor)
    {
        this._processor = processor;
    }

    public async Task<BusinessResult<AssetCategoryDeletionUiModel>> Handle(DeleteHardAssetCategoryCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteHardAssetCategoryAsync(deleteCommand);
    }
}
