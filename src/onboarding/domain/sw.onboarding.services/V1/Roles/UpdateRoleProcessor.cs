using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Roles;
using sw.auth.model.Roles;
using sw.onboarding.common.dtos.Cqrs.Roles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.onboarding.services.V1.Roles;

public class UpdateRoleProcessor : IUpdateRoleProcessor,
    IRequestHandler<UpdateRoleCommand, BusinessResult<RoleModificationUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IRoleRepository _roleRepository;
    private readonly IAutoMapper _autoMapper;
    public UpdateRoleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
        IRoleRepository roleRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        _roleRepository = roleRepository;
    }

    public async Task<BusinessResult<RoleModificationUiModel>> Handle(UpdateRoleCommand updateCommand, CancellationToken cancellationToken)
    {
        return await UpdateRoleAsync(updateCommand);
    }

    public async Task<BusinessResult<RoleModificationUiModel>> UpdateRoleAsync(UpdateRoleCommand updateCommand)
    {
        var bc = new BusinessResult<RoleModificationUiModel>(new RoleModificationUiModel());

        if (updateCommand.IsNull())
        {
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError("ERROR_UPDATE_COMMAND_MODEL"));
            return await Task.FromResult(bc);
        }

        try
        {
            var role = _roleRepository.FindBy(updateCommand.Id);
            if (role is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Role Id does not exist"));
                return bc;
            }

            role.ModifyWithAudit(updateCommand.ModifiedById, updateCommand);


            Persist(role, updateCommand.Id);

            var response = _autoMapper.Map<RoleModificationUiModel>(role);

            response.Message = $"Role id: {response.Id} updated successfully";

            bc.Model = response;
        }
        catch (Exception e)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.Model = null;
            bc.AddBrokenRule(new BusinessError(errorMessage));
            Log.Error(
                $"Update Role: {updateCommand.Name}" +
                $"Error Message:{errorMessage}" +
                $"--UpdateRole--  @fail@ [UpdateRoleProcessor]. " +
                $"@innerfault:{e.Message} and {e.InnerException}");
        }

        return await Task.FromResult(bc);

        //if (updateCommand is null)
        //{
        //    bc.AddBrokenRule(new BusinessError(null));
        //}

        //var role = _roleRepository.FindBy(updateCommand.Id);
        //if (role is null)
        //{
        //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Role Id does not exist"));
        //    return bc;
        //}

        //var modifiedRole = _autoMapper.Map<Role>(updateCommand);
        //role.Modified(updateCommand.ModifiedById, modifiedRole);

        //Persist(role, updateCommand.Id);

        //var response = _autoMapper.Map<RoleModificationUiModel>(role);
        //response.Message = $"Role id: {response.Id} updated successfully";

        //bc.Model = response;

        //return await Task.FromResult(bc);
    }

    private void Persist(Role role, long id)
    {
        this._roleRepository.Save(role, id);
        this._uOf.Commit();
    }
}