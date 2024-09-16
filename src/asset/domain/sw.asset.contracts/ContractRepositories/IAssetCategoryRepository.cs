using sw.asset.model;
using sw.asset.model.Assets.Categories;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.asset.contracts.ContractRepositories
{
    public interface IAssetCategoryRepository : IRepository<AssetCategory, long>
    {
        QueryResult<AssetCategory> FindAllActivePagedOf(int? pageNum, int? pageSize);
        AssetCategory FindActiveById(long id);
    }
}
