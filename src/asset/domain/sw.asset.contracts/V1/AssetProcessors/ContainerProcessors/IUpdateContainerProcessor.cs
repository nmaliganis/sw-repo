using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;

public interface IUpdateContainerProcessor
{
    Task<BusinessResult<ContainerUiModel>> UpdateContainerAsync(UpdateContainerCommand updateCommand);
    Task<BusinessResult<ContainerModificationUiModel>> UpdateContainerWithLatLotAsync(UpdateContainerWithLatLonCommand updateCommand);
    Task<BusinessResult<ContainerModificationUiModel>> UpdateContainerWithLatLotAsync(UpdateContainerWithLatLonByDeviceCommand updateCommand);
    Task<BusinessResult<ContainerModificationUiModel>> UpdateContainerWithLatLotAsync(UpdateContainerByNameWithLatLonCommand updateCommand);
    Task<BusinessResult<ContainerUiModel>> UpdateContainerWithMeasurementsAsync(UpdateContainerMeasurementsCommand updateCommand);
    Task<BusinessResult<ContainerUiModel>> UpdateContainerWithMeasurementsForUltrasonicAsync(UpdateContainerMeasurementsCommand updateCommand);
    Task<BusinessResult<ContainerUiModel>> UpdateContainerWithMeasurementsForMotionAsync(UpdateContainerMeasurementsCommand updateCommand);
    Task<BusinessResult<ContainerModificationUiModel>> OnboardingContainerWithDeviceAsync(long containerId, long deviceId, OnboardingContainerWithDeviceCommand updateCommand);
    Task<BusinessResult<ContainerUiModel>> OnboardingContainerByNameWithDeviceAsync(string containerName, string deviceImei, long modifiedBy);
}