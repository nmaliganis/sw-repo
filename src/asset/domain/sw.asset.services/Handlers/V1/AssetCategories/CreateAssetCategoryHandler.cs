using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.AssetCategories;
using sw.asset.common.dtos.Vms.AssetCategories;
using sw.asset.contracts.V1.AssetCategoryProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.AssetCategories;

internal class CreateAssetCategoryHandler :
    IRequestHandler<CreateAssetCategoryCommand, BusinessResult<AssetCategoryCreationUiModel>>
{
    private readonly ICreateAssetCategoryProcessor _processor;

    public CreateAssetCategoryHandler(ICreateAssetCategoryProcessor processor)
    {
        this._processor = processor;
    }

    public async Task<BusinessResult<AssetCategoryCreationUiModel>> Handle(CreateAssetCategoryCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateAssetCategoryAsync(createCommand);
    }
}
