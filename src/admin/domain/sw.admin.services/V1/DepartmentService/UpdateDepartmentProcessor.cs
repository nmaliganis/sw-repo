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
    public class UpdateDepartmentProcessor : IUpdateDepartmentProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateDepartmentProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
            IDepartmentRepository departmentRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _departmentRepository = departmentRepository;
        }

        public async Task<BusinessResult<DepartmentModificationUiModel>> UpdateDepartmentAsync(UpdateDepartmentCommand updateCommand)
        {
            var bc = new BusinessResult<DepartmentModificationUiModel>(new DepartmentModificationUiModel());

            if (updateCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var department = _departmentRepository.FindBy(updateCommand.Id);
            if (department is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "Department Id does not exist"));
                return bc;
            }

            var modifiedDepartment = _autoMapper.Map<Department>(updateCommand);
            department.Modified(updateCommand.ModifiedById, modifiedDepartment);

            Persist(department, updateCommand.Id);

            var response = _autoMapper.Map<DepartmentModificationUiModel>(department);
            response.Message = $"Department id: {response.Id} updated successfully";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        private void Persist(Department department, long id)
        {
            _departmentRepository.Save(department, id);
            _uOf.Commit();
        }
    }
}
