using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class OnboardingContainerWithDeviceByNameHandler :
  IRequestHandler<OnboardingContainerWithDeviceByNameCommand, BusinessResult<ContainerUiModel>>
{
  private readonly IUpdateContainerProcessor _processor;

  public OnboardingContainerWithDeviceByNameHandler(IUpdateContainerProcessor processor)
  {
    _processor = processor;
  }

  public async Task<BusinessResult<ContainerUiModel>> Handle(OnboardingContainerWithDeviceByNameCommand updateCommand, CancellationToken cancellationToken)
  {
    return await _processor.OnboardingContainerByNameWithDeviceAsync(updateCommand.ContainerName, updateCommand.DeviceImei, updateCommand.ModifiedById);
  }
}