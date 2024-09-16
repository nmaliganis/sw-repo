using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.admin.services.V1.DepartmentService
{
    public class CreateDepartmentProcessor : ICreateDepartmentProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAutoMapper _autoMapper;

        public CreateDepartmentProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IDepartmentRepository departmentRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _departmentRepository = departmentRepository;
        }

        public async Task<BusinessResult<DepartmentCreationUiModel>> CreateDepartmentAsync(CreateDepartmentCommand createCommand)
        {
            var bc = new BusinessResult<DepartmentCreationUiModel>(new DepartmentCreationUiModel());

            if (createCommand is null)
            {
                bc.Model = null; 
                bc.AddBrokenRule(new BusinessError(null));
                return await Task.FromResult(bc);
            }

            var nameExists = _departmentRepository.FindDepartmentByName(createCommand.Parameters.Name);
            if (nameExists != null)
            {
                bc.Model = null;
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(createCommand.Parameters.Name), "ERROR_DEPARTMENT_ALREADY_EXISTS"));
                return await Task.FromResult(bc);

            }

            var department =_autoMapper.Map<Department>(createCommand);
            department.Created(createCommand.CreatedById);

            Persist(department);

            var response = _autoMapper.Map<DepartmentCreationUiModel>(department);
            response.Message = "Department created successfully.";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Department department)
        {
            _departmentRepository.Add(department);
            _uOf.Commit();
        }
    }
}
