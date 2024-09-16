using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.DepartmentProcessors;
using sw.common.dtos.Vms.Departments;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;
using sw.auth.common.dtos.Vms.Departments;

namespace sw.auth.services.V1.DepartmentService
{
    public class GetDepartmentByIdProcessor :
        IGetDepartmentByIdProcessor,
        IRequestHandler<GetDepartmentByIdQuery, BusinessResult<DepartmentUiModel>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentByIdProcessor(IDepartmentRepository departmentRepository, IAutoMapper autoMapper)
        {
            _departmentRepository = departmentRepository;
            _autoMapper = autoMapper;
        }

        public async Task<BusinessResult<DepartmentUiModel>> Handle(GetDepartmentByIdQuery qry, CancellationToken cancellationToken)
        {
            return await GetDepartmentByIdAsync(qry.Id);
        }

        public async Task<BusinessResult<DepartmentUiModel>> GetDepartmentByIdAsync(long id)
        {
            var bc = new BusinessResult<DepartmentUiModel>(new DepartmentUiModel());

            var department = _departmentRepository.FindActiveById(id);
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
