using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.admin.services.V1.DepartmentPersonRoleService
{
    public class DeleteSoftDepartmentPersonRoleProcessor : IDeleteSoftDepartmentPersonRoleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentPersonRoleRepository _departmentPersonRoleRepository;

        public DeleteSoftDepartmentPersonRoleProcessor(IUnitOfWork uOf, IDepartmentPersonRoleRepository departmentPersonRoleRepository)
        {
            _uOf = uOf;
            _departmentPersonRoleRepository = departmentPersonRoleRepository;
        }

        public async Task<BusinessResult<DepartmentPersonRoleDeletionUiModel>> DeleteSoftDepartmentPersonRoleAsync(DeleteSoftDepartmentPersonRoleCommand deleteCommand)
        {
            var bc = new BusinessResult<DepartmentPersonRoleDeletionUiModel>(new DepartmentPersonRoleDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var departmentPersonRole = _departmentPersonRoleRepository.FindBy(deleteCommand.Id);
            if (departmentPersonRole is null || !departmentPersonRole.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "DepartmentPersonRole Id does not exist"));
                return bc;
            }

            departmentPersonRole.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(departmentPersonRole, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"DepartmentPersonRole with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(DepartmentPersonRole departmentPersonRole, long id)
        {
            _departmentPersonRoleRepository.Save(departmentPersonRole, id);
            _uOf.Commit();
        }
    }
}
