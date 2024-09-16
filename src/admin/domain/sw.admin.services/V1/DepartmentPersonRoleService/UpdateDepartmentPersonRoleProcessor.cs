using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.admin.services.V1.DepartmentPersonRoleService
{
    public class UpdateDepartmentPersonRoleProcessor : IUpdateDepartmentPersonRoleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentPersonRoleRepository _departmentPersonRoleRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateDepartmentPersonRoleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IDepartmentPersonRoleRepository departmentPersonRoleRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _departmentPersonRoleRepository = departmentPersonRoleRepository;
        }

        public async Task<BusinessResult<DepartmentPersonRoleModificationUiModel>> UpdateDepartmentPersonRoleAsync(UpdateDepartmentPersonRoleCommand updateCommand)
        {
            var bc = new BusinessResult<DepartmentPersonRoleModificationUiModel>(new DepartmentPersonRoleModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var departmentPersonRole = _departmentPersonRoleRepository.FindBy(updateCommand.Id);
            if (departmentPersonRole is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "DepartmentPersonRole Id does not exist"));
                return bc;
            }

            var modifiedDepartmentPersonRole = _autoMapper.Map<DepartmentPersonRole>(updateCommand);
            departmentPersonRole.Modified(updateCommand.ModifiedById, modifiedDepartmentPersonRole);

            Persist(departmentPersonRole, updateCommand.Id);

            var response = _autoMapper.Map<DepartmentPersonRoleModificationUiModel>(departmentPersonRole);
            response.Message = $"DepartmentPersonRole id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(DepartmentPersonRole departmentPersonRole, long id)
        {
            _departmentPersonRoleRepository.Save(departmentPersonRole, id);
            _uOf.Commit();
        }
    }
}
