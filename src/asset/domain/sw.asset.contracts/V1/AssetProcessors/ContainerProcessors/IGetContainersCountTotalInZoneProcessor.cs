using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.infrastructure.BrokenRules;

namespace sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;

public interface IGetContainersCountTotalInZoneProcessor
{
    Task<BusinessResult<ContainerCountUiModel>> GetContainersCountTotalInZoneAsync(GetContainersCountTotalInZonesQuery qry);
}