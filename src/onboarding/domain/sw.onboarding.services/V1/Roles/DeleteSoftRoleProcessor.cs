using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Roles;
using sw.auth.model.Roles;
using sw.onboarding.common.dtos.Cqrs.Roles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;

namespace sw.onboarding.services.V1.Roles;

public class DeleteSoftRoleProcessor :
    IDeleteSoftRoleProcessor,
    IRequestHandler<DeleteSoftRoleCommand, BusinessResult<RoleDeletionUiModel>> {
    private readonly IUnitOfWork _uOf;
    private readonly IRoleRepository _roleRepository;

    public DeleteSoftRoleProcessor(IUnitOfWork uOf, IRoleRepository roleRepository) {
        this._uOf = uOf;
        this._roleRepository = roleRepository;
    }

    public async Task<BusinessResult<RoleDeletionUiModel>> Handle(DeleteSoftRoleCommand deleteCommand, CancellationToken cancellationToken) {
        return await this.DeleteSoftRoleAsync(deleteCommand);
    }

    public async Task<BusinessResult<RoleDeletionUiModel>> DeleteSoftRoleAsync(DeleteSoftRoleCommand deleteCommand) {
        var bc = new BusinessResult<RoleDeletionUiModel>(new RoleDeletionUiModel());

        if (deleteCommand is null) {
            bc.AddBrokenRule(new BusinessError(null));
        }

        var role = this._roleRepository.FindBy(deleteCommand.Id);
        if (role is null || !role.Active) {
            bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Role Id does not exist"));
            return bc;
        }

        role.DeleteWithAudit(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

        this.Persist(role, deleteCommand.Id);

        bc.Model.Id = deleteCommand.Id;
        bc.Model.Active = false;

        //bc.Model.Hard = false;
        bc.Model.Message = $"Role with id: {deleteCommand.Id} has been deleted successfully.";

        return await Task.FromResult(bc);
    }

    private void Persist(Role role, long id) {
        this._roleRepository.Save(role, id);
        this._uOf.Commit();
    }
}//Class : DeleteSoftRoleProcessor
//Namespace : sw.auth.services.V1.Roles
