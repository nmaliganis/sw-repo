using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using System.Threading.Tasks;
using sw.landmark.common.dtos.V1.Vms.Departments;

namespace sw.admin.services.V1.DepartmentService
{
    public class GetDepartmentByIdProcessor : IGetDepartmentByIdProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentByIdProcessor(IDepartmentRepository departmentRepository, IAutoMapper autoMapper)
        {
            _departmentRepository = departmentRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<DepartmentUiModel>> GetDepartmentByIdAsync(long id)
        {
            var bc = new BusinessResult<DepartmentUiModel>(new DepartmentUiModel());

            var department = _departmentRepository.FindActiveBy(id);
            if (department is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "Department Id does not exist"));
                return bc;
            }

            var response = _autoMapper.Map<DepartmentUiModel>(department);
            response.Message = $"Department id: {response.Id} fetched successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }
    }
}
