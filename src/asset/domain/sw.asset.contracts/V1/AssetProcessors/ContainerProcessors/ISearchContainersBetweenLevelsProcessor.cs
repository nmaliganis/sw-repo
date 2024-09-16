using System.Collections.Generic;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.infrastructure.BrokenRules;

namespace sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;

public interface ISearchContainersBetweenLevelsProcessor
{
    Task<BusinessResult<List<ContainerUiModel>>> SearchContainersBetweenLevelsAsync(GetContainersByVolumeInZonesQuery qry);
}