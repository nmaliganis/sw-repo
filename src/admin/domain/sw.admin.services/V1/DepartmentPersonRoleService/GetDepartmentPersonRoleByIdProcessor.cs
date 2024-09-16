using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;

namespace sw.admin.services.V1.DepartmentPersonRoleService
{
    public class GetDepartmentPersonRoleByIdProcessor : IGetDepartmentPersonRoleByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDepartmentPersonRoleRepository _departmentPersonRoleRepository;

        public GetDepartmentPersonRoleByIdProcessor(IDepartmentPersonRoleRepository departmentPersonRoleRepository, IAutoMapper autoMapper)
        {
            _departmentPersonRoleRepository = departmentPersonRoleRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<DepartmentPersonRoleUiModel>> GetDepartmentPersonRoleByIdAsync(long id)
        {
            var bc = new BusinessResult<DepartmentPersonRoleUiModel>(new DepartmentPersonRoleUiModel());

            var departmentPersonRole = _departmentPersonRoleRepository.FindActiveBy(id);
            if (departmentPersonRole is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "DepartmentPersonRole Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<DepartmentPersonRoleUiModel>(departmentPersonRole);
            response.Message = $"DepartmentPersonRole id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
