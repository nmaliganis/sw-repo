using System;
using sw.asset.common.dtos.Cqrs.Assets.Containers;
using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Threading.Tasks;
using sw.infrastructure.Extensions;
using Serilog;

namespace sw.asset.services.V1.AssetServices.ContainerService;

public class SearchContainersBetweenLevelsProcessor : ISearchContainersBetweenLevelsProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IContainerRepository _containerRepository;

    public SearchContainersBetweenLevelsProcessor(IContainerRepository containerRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        this._containerRepository = containerRepository;
        this._autoMapper = autoMapper;
        this._propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<List<ContainerUiModel>>> SearchContainersBetweenLevelsAsync(GetContainersByVolumeInZonesQuery qry)
    {
        var bc = new BusinessResult<List<ContainerUiModel>>(new List<ContainerUiModel>());

        try
        {
            var containers = await _containerRepository.SearchNativeBetweenLevel(qry.Zones, qry.LowerLevel, qry.UpperLevel);
            if (containers.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_SEARCH_VOLUME_MODEL"));
                return await Task.FromResult(bc);
            }

            var items = _autoMapper.Map<List<ContainerUiModel>>(containers);

            bc.Model = items;
        }
        catch (Exception e)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(qry), "SearchContainersBetweenLevelsAsync"));
            Log.Error(
                $"Search Containers With Criteria: LowerLevel - {qry.LowerLevel} - UpperLevel - {qry.UpperLevel}" +
                $"SearchContainersBetweenLevelsAsync " +
                $"Exception message: {e.Message} --SearchContainersBetweenLevelsAsync--  @NotComplete@ [SearchContainersBetweenLevelsProcessor].");
            return bc;
        }
        return await Task.FromResult(bc);
    }

}//Class: SearchContainersBetweenLevelsProcessor