using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class OnboardingContainerWithDeviceHandler :
    IRequestHandler<OnboardingContainerWithDeviceCommand, BusinessResult<ContainerModificationUiModel>>
{
    private readonly IUpdateContainerProcessor _processor;

    public OnboardingContainerWithDeviceHandler(IUpdateContainerProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerModificationUiModel>> Handle(OnboardingContainerWithDeviceCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.OnboardingContainerWithDeviceAsync(updateCommand.ContainerId, updateCommand.DeviceId, updateCommand);
    }
}