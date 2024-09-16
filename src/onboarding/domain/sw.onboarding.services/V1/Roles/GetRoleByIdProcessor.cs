using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Roles;
using sw.onboarding.common.dtos.Cqrs.Roles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using MediatR;
using Serilog;

namespace sw.onboarding.services.V1.Roles;

public class GetRoleByIdProcessor : IGetRoleByIdProcessor,
  IRequestHandler<GetRoleByIdQuery, BusinessResult<RoleUiModel>>
{
  private readonly IAutoMapper _autoMapper;
  private readonly IRoleRepository _roleRepository;
  public GetRoleByIdProcessor(IRoleRepository roleRepository, IAutoMapper autoMapper)
  {
    _roleRepository = roleRepository;
    _autoMapper = autoMapper;
  }

  public async Task<BusinessResult<RoleUiModel>> GetRoleByIdAsync(long id)
  {
    var bc = new BusinessResult<RoleUiModel>(new RoleUiModel());

    var role = this._roleRepository.FindActiveById(id);
    if (role.IsNull())
    {
      Log.Information(
        $"--Method:GetRoleByIdAsync -- Message:ROLE_FETCH" +
        $" -- Datetime:{DateTime.Now} -- Just After : _roleRepository.FindBy");
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Role Id does not exist"));
      return bc;
    }

    var response = this._autoMapper.Map<RoleUiModel>(role);
    response.Message = $"Role id: {response.Id} fetched successfully";

    bc.Model = response;

    return await Task.FromResult(bc);
  }

  public async Task<BusinessResult<RoleUiModel>> Handle(GetRoleByIdQuery qry, CancellationToken cancellationToken)
  {
    return await GetRoleByIdAsync(qry.Id);
  }
}