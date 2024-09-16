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

public class DeleteHardRoleProcessor :
  IDeleteHardRoleProcessor,
  IRequestHandler<DeleteHardRoleCommand, BusinessResult<RoleDeletionUiModel>> {
  private readonly IUnitOfWork _uOf;
  private readonly IRoleRepository _roleRepository;

  public DeleteHardRoleProcessor(IUnitOfWork uOf, IRoleRepository roleRepository) {
    this._uOf = uOf;
    this._roleRepository = roleRepository;
  }

  public async Task<BusinessResult<RoleDeletionUiModel>> Handle(DeleteHardRoleCommand deleteCommand, CancellationToken cancellationToken) {
    return await this.DeleteHardRoleAsync(deleteCommand);
  }

  public async Task<BusinessResult<RoleDeletionUiModel>> DeleteHardRoleAsync(DeleteHardRoleCommand deleteCommand) {
    var bc = new BusinessResult<RoleDeletionUiModel>(new RoleDeletionUiModel());

    if (deleteCommand is null) {
      bc.AddBrokenRule(new BusinessError(null));
    }

    var role = this._roleRepository.FindBy(deleteCommand.Id);
    if (role is null) {
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Role Id does not exist"));
      return bc;
    }

    this.Persist(role);

    //bc.Model.Id = deleteCommand.Id;
    //bc.Model.Successful = true;
    //bc.Model.Hard = true;
    bc.Model.Message = $"Role with id: {deleteCommand.Id} has been hard deleted successfully.";

    return await Task.FromResult(bc);
  }

  private void Persist(Role role) {
    this._roleRepository.Remove(role);
    this._uOf.Commit();
  }
}//Class : DeleteHardRoleProcessor
