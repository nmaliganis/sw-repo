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
    public class CreateDepartmentPersonRoleProcessor : ICreateDepartmentPersonRoleProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentPersonRoleRepository _departmentPersonRoleRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateDepartmentPersonRoleProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IDepartmentPersonRoleRepository departmentPersonRoleRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _departmentPersonRoleRepository = departmentPersonRoleRepository;
        }

        public async Task<BusinessResult<DepartmentPersonRoleCreationUiModel>> CreateDepartmentPersonRoleAsync(CreateDepartmentPersonRoleCommand createCommand)
        {
            var bc = new BusinessResult<DepartmentPersonRoleCreationUiModel>(new DepartmentPersonRoleCreationUiModel());

            if (createCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            //var nameExists = _departmentPersonRoleRepository.HasName(createCommand.Name);
            //if (nameExists)
            //{
            //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Name), "DepartmentPersonRole name already exists"));
            //}

            var departmentPersonRole =_autoMapper.Map<DepartmentPersonRole>(createCommand);
            departmentPersonRole.Created(createCommand.CreatedById);

            Persist(departmentPersonRole);

            var response = _autoMapper.Map<DepartmentPersonRoleCreationUiModel>(departmentPersonRole);
            response.Message = "DepartmentPersonRole created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(DepartmentPersonRole departmentPersonRole)
        {
            _departmentPersonRoleRepository.Add(departmentPersonRole);
            _uOf.Commit();
        }
    }
}
