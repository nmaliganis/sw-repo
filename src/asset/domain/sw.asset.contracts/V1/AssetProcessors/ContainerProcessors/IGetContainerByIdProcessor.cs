using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetProcessors.ContainerProcessors
{
    public interface IGetContainerByIdProcessor
    {
        Task<BusinessResult<ContainerUiModel>> GetContainerByIdAsync(long id);
    }
}
