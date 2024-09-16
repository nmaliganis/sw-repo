using sw.asset.common.dtos.Vms.Assets.Containers;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.AssetProcessors.ContainerProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.asset.services.V1.AssetServices.ContainerService;

public class GetContainerByIdProcessor :
    IGetContainerByIdProcessor
{
    private readonly IAutoMapper _autoMapper;
    private readonly IContainerRepository _containerRepository;
    public GetContainerByIdProcessor(IContainerRepository containerRepository, IAutoMapper autoMapper)
    {
        _containerRepository = containerRepository;
        _autoMapper = autoMapper;
    }

    public async Task<BusinessResult<ContainerUiModel>> GetContainerByIdAsync(long id)
    {
        var bc = new BusinessResult<ContainerUiModel>(new ContainerUiModel());

        var container = _containerRepository.FindActiveById(id);
        if (container is null)
        {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Container Id does not exist"));
            return bc;
        }

        var response = _autoMapper.Map<ContainerUiModel>(container);
        response.Message = $"Container id: {response.Id} fetched successfully";

        bc.Model = response;

        return await Task.FromResult(bc);
    }
}// Class: GetContainerByIdProcessor