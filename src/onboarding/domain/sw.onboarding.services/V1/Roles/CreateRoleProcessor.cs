using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.common.infrastructure.Exceptions.Roles;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Roles;
using sw.auth.model.Roles;
using sw.onboarding.common.dtos.Cqrs.Roles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.onboarding.services.V1.Roles;
public class CreateRoleProcessor :
  ICreateRoleProcessor,
  IRequestHandler<CreateRoleCommand, BusinessResult<RoleUiModel>>
{
  private readonly IUnitOfWork _uOf;
  private readonly IRoleRepository _roleRepository;
  private readonly IAutoMapper _autoMapper;

  public CreateRoleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IRoleRepository roleRepository)
  {
    this._uOf = uOf;
    this._autoMapper = autoMapper;
    this._roleRepository = roleRepository;
  }

  public async Task<BusinessResult<RoleUiModel>> Handle(CreateRoleCommand createCommand, CancellationToken cancellationToken)
  {
    return await this.CreateRoleAsync(createCommand);
  }

  public async Task<BusinessResult<RoleUiModel>> CreateRoleAsync(CreateRoleCommand createCommand)
  {
    var bc = new BusinessResult<RoleUiModel>(new RoleUiModel());

    if (createCommand is null)
    {
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError("ERROR_CREATE_COMMAND_MODEL"));
      return await Task.FromResult(bc);
    }

    try
    {
      var roleToBeCreated = new Role();

      roleToBeCreated.InjectWithInitialAttributes(createCommand);
      roleToBeCreated.InjectWithAudit(createCommand.CreatedById);

      this.ThrowExcIfRoleCannotBeCreated(roleToBeCreated);
      this.ThrowExcIfThisRoleAlreadyExist(roleToBeCreated);

      Log.Debug(
        $"Create Role: {createCommand.Name}" +
        "--CreateRole--  @NotComplete@ [CreateRoleProcessor]. " +
        "Message: Just Before MakeItPersistence");

      MakeRolePersistent(roleToBeCreated);

      Log.Debug(
        $"Create Role: {createCommand.Name}" +
        "--CreateRole--  @NotComplete@ [CreateRoleProcessor]. " +
        "Message: Just After MakeItPersistence");
      bc.Model = ThrowExcIfRoleWasNotBeMadePersistent(roleToBeCreated);
    }
    catch (InvalidRoleException e)
    {
      string errorMessage = "ERROR_INVALID_Role_MODEL";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
        $"Create Role: {createCommand.Name}" +
        $"Error Message:{errorMessage}" +
        "--CreateRole--  @NotComplete@ [CreateRoleProcessor]. " +
        $"Broken rules: {e.BrokenRules}");
    }
    catch (RoleAlreadyExistsException ex)
    {
      string errorMessage = "ERROR_ROLE_ALREADY_EXISTS";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
        $"Create Role: {createCommand.Name}" +
        $"Error Message:{errorMessage}" +
        "--CreateRole--  @fail@ [CreateRoleProcessor]. " +
        $"@innerfault:{ex?.Message} and {ex?.InnerException}");
    }
    catch (RoleDoesNotExistAfterMadePersistentException exx)
    {
      string errorMessage = "ERROR_ROLE_NOT_MADE_PERSISTENT";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
        $"Create Role: {createCommand.Name}" +
        $"Error Message:{errorMessage}" +
        "--CreateRole--  @fail@ [CreateRoleProcessor]." +
        $" @innerfault:{exx?.Message} and {exx?.InnerException}");
    }
    catch (Exception exxx)
    {
      string errorMessage = "UNKNOWN_ERROR";
      bc.Model = null;
      bc.AddBrokenRule(new BusinessError(errorMessage));
      Log.Error(
        $"Create Role: {createCommand.Name}" +
        $"Error Message:{errorMessage}" +
        $"--CreateRole--  @fail@ [CreateRoleProcessor]. " +
        $"@innerfault:{exxx.Message} and {exxx.InnerException}");
    }

    return await Task.FromResult(bc);
  }

  private void ThrowExcIfThisRoleAlreadyExist(Role roleToBeCreated)
  {
    var roleRetrieved = this._roleRepository.FindRoleByName(roleToBeCreated.Name);
    if (roleRetrieved != null)
    {
      throw new RoleAlreadyExistsException(roleToBeCreated.Name,
        roleToBeCreated.GetBrokenRulesAsString());
    }
  }

  private RoleUiModel ThrowExcIfRoleWasNotBeMadePersistent(Role roleToBeCreated)
  {
    var retrievedRole = this._roleRepository.FindRoleByName(roleToBeCreated.Name);
    if (retrievedRole != null)
    {
      return this._autoMapper.Map<RoleUiModel>(retrievedRole);
    }

    throw new RoleDoesNotExistAfterMadePersistentException(roleToBeCreated.Name);
  }

  private void ThrowExcIfRoleCannotBeCreated(Role roleToBeCreated)
  {
    bool canBeCreated = !roleToBeCreated.GetBrokenRules().Any();
    if (!canBeCreated)
    {
      throw new InvalidRoleException(roleToBeCreated.GetBrokenRulesAsString());
    }
  }
  private void MakeRolePersistent(Role role)
  {
    this._roleRepository.Save(role);
    this._uOf.Commit();
  }
}//Class : CreateRoleProcessor