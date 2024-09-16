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

public class SearchContainersWithCriteriaProcessor : ISearchContainersWithCriteriaProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IContainerRepository _containerRepository;

    public SearchContainersWithCriteriaProcessor(IContainerRepository containerRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        this._containerRepository = containerRepository;
        this._autoMapper = autoMapper;
        this._propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<List<ContainerUiModel>>> SearchContainersWithCriteriaAsync(GetContainersByCriteriaInZonesQuery qry)
    {
        var bc = new BusinessResult<List<ContainerUiModel>>(new List<ContainerUiModel>());

        try
        {
            var containers = await _containerRepository.SearchNativeWithCriteria(qry.Zones, qry.Criteria);
            if (containers.IsNull())
            {
                bc.Model = null;
                bc.AddBrokenRule(new BusinessError("ERROR_SEARCH_CRITERIA_MODEL"));
                return await Task.FromResult(bc);
            }

            var items = _autoMapper.Map<List<ContainerUiModel>>(containers);

            bc.Model = items;
        }
        catch (Exception e)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(qry), "SearchContainersWithCriteriaAsync"));
            Log.Error(
                $"Search Containers With Criteria: {qry.Criteria}" +
                $"SearchContainersWithCriteriaAsync " +
                $"Exception message: {e.Message} --SearchContainersWithCriteriaAsync--  @NotComplete@ [SearchContainersWithCriteriaProcessor].");
            return bc;
        }
        return await Task.FromResult(bc);
    }

}//Class: SearchContainersWithCriteriaProcessor