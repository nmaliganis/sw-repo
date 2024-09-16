using System.Collections.Generic;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;

public interface IGetContainersProcessor
{
    Task<BusinessResult<PagedList<ContainerUiModel>>> GetContainersAsync(GetContainersQuery qry);
    Task<BusinessResult<PagedList<ContainerUiModel>>> GetContainersByZoneIdAsync(long zoneId, GetContainersByZoneIdQuery qry);
    Task<BusinessResult<List<ContainerUiModel>>> GetContainersByZonesAsync(List<long> zones);
}