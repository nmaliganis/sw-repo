using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers.Types;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.ContainerService;

public class GetContainersCountTotalProcessor : IGetContainersCountTotalProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IContainerRepository _containerRepository;

    public GetContainersCountTotalProcessor(IContainerRepository containerRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        this._containerRepository = containerRepository;
        this._autoMapper = autoMapper;
        this._propertyMappingService = propertyMappingService;
    }
    public Task<BusinessResult<ContainerCountUiModel>> GetContainersCountTotalAsync(GetContainersCountTotalQuery qry)
    {
        var containerCount = new ContainerCountUiModel();
        var bc = new BusinessResult<ContainerCountUiModel>(containerCount);

        containerCount.TotalCount = _containerRepository.FindCountTotal();

        Dictionary<ContainerType, int> typeCounts = new Dictionary<ContainerType, int>();

        var totalCountTrash = _containerRepository.FindCountPerContainerType((int)ContainerType.Trash);
        typeCounts.Add(ContainerType.Trash, totalCountTrash);
        var totalCountRecycle = _containerRepository.FindCountPerContainerType((int)ContainerType.Recycle);
        typeCounts.Add(ContainerType.Recycle, totalCountRecycle);
        var totalCountCompost = _containerRepository.FindCountPerContainerType((int)ContainerType.Compost);
        typeCounts.Add(ContainerType.Compost, totalCountCompost);
        var totalCountOther = _containerRepository.FindCountPerContainerType((int)ContainerType.Other);
        typeCounts.Add(ContainerType.Other, totalCountOther);

        containerCount.Counts = typeCounts;

        return Task.FromResult(bc);
    }

}//Class: GetContainersCountTotalProcessor