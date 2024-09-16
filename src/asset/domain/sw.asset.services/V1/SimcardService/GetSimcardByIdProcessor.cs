using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.SimcardProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;

namespace sw.asset.services.V1.SimcardService;

public class GetSimcardByIdProcessor :
  IGetSimcardByIdProcessor,
  IRequestHandler<GetSimcardByIdQuery, BusinessResult<SimcardUiModel>>
{
  private readonly IAutoMapper _autoMapper;
  private readonly ISimcardRepository _simcardRepository;

  public GetSimcardByIdProcessor(ISimcardRepository simcardRepository, IAutoMapper autoMapper)
  {
    _simcardRepository = simcardRepository;
    _autoMapper = autoMapper;
  }

  public async Task<BusinessResult<SimcardUiModel>> Handle(GetSimcardByIdQuery qry, CancellationToken cancellationToken)
  {
    return await GetSimcardByIdAsync(qry.Id);
  }

  public async Task<BusinessResult<SimcardUiModel>> GetSimcardByIdAsync(long id)
  {
    var bc = new BusinessResult<SimcardUiModel>(new SimcardUiModel());

    var simcard = _simcardRepository.FindOneActiveById(id);
    if (simcard is null)
    {
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Simcard Id does not exist"));
      return bc;
    }

    var response = _autoMapper.Map<SimcardUiModel>(simcard);
    response.Message = $"Simcard id: {response.Id} fetched successfully";

    bc.Model = response;

    return await Task.FromResult(bc);
  }
}