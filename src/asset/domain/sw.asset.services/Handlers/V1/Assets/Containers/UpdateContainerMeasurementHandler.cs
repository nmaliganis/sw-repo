using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.asset.services.Handlers.V1.Assets.Containers;

internal class UpdateContainerMeasurementHandler :
  IRequestHandler<UpdateContainerMeasurementsCommand, BusinessResult<ContainerUiModel>>
{
    private readonly IUpdateContainerProcessor _processor;

    public UpdateContainerMeasurementHandler(IUpdateContainerProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<ContainerUiModel>> Handle(UpdateContainerMeasurementsCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateContainerWithMeasurementsAsync(updateCommand);
    }
}