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

public class GetContainersCountTotalInZoneProcessor : IGetContainersCountTotalInZoneProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IContainerRepository _containerRepository;

    public GetContainersCountTotalInZoneProcessor(IContainerRepository containerRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        this._containerRepository = containerRepository;
        this._autoMapper = autoMapper;
        this._propertyMappingService = propertyMappingService;
    }
    public Task<BusinessResult<ContainerCountUiModel>> GetContainersCountTotalInZoneAsync(GetContainersCountTotalInZonesQuery qry)
    {
        var containerCount = new ContainerCountUiModel();
        var bc = new BusinessResult<ContainerCountUiModel>(containerCount);

        containerCount.TotalCount = _containerRepository.FindCountInZonesTotal(qry.Zones);

        Dictionary<ContainerType, int> typeCounts = new Dictionary<ContainerType, int>();

        var totalCountTrash = _containerRepository.FindCountPerContainerTypeInZones((int)ContainerType.Trash, qry.Zones);
        typeCounts.Add(ContainerType.Trash, totalCountTrash);
        var totalCountRecycle = _containerRepository.FindCountPerContainerTypeInZones((int)ContainerType.Recycle, qry.Zones);
        typeCounts.Add(ContainerType.Recycle, totalCountRecycle);
        var totalCountCompost = _containerRepository.FindCountPerContainerTypeInZones((int)ContainerType.Compost, qry.Zones);
        typeCounts.Add(ContainerType.Compost, totalCountCompost);
        var totalCountOther = _containerRepository.FindCountPerContainerTypeInZones((int)ContainerType.Other, qry.Zones);
        typeCounts.Add(ContainerType.Other, totalCountOther);

        containerCount.Counts = typeCounts;

        return Task.FromResult(bc);
    }

}//Class: GetContainersCountTotalInZoneProcessor