using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.AssetCategories;

internal class UpdateAssetCategoryHandler :
    IRequestHandler<UpdateAssetCategoryCommand, BusinessResult<AssetCategoryModificationUiModel>>
{
    private readonly IUpdateAssetCategoryProcessor _processor;

    public UpdateAssetCategoryHandler(IUpdateAssetCategoryProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<AssetCategoryModificationUiModel>> Handle(UpdateAssetCategoryCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateAssetCategoryAsync(updateCommand);
    }
    }
