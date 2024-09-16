using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using System.Threading.Tasks;

namespace sw.admin.services.V1.DepartmentService
{
    public class DeleteHardDepartmentProcessor : IDeleteHardDepartmentProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteHardDepartmentProcessor(IUnitOfWork uOf, IDepartmentRepository departmentRepository)
        {
            _uOf = uOf;
            _departmentRepository = departmentRepository;
        }

        public async Task<BusinessResult<DepartmentDeletionUiModel>> DeleteHardDepartmentAsync(DeleteHardDepartmentCommand deleteCommand)
        {
            var bc = new BusinessResult<DepartmentDeletionUiModel>(new DepartmentDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var department = _departmentRepository.FindBy(deleteCommand.Id);
            if (department is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Department Id does not exist"));
                return bc;
            }

            Persist(department);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = true;
            bc.Model.Message = $"Department with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Department department)
        {
            _departmentRepository.Remove(department);
            _uOf.Commit();
        }
    }
}
