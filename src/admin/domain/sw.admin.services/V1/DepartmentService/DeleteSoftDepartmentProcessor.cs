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
    public class DeleteSoftDepartmentProcessor : IDeleteSoftDepartmentProcessor
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteSoftDepartmentProcessor(IUnitOfWork uOf, IDepartmentRepository departmentRepository)
        {
            _uOf = uOf;
            _departmentRepository = departmentRepository;
        }

        public async Task<BusinessResult<DepartmentDeletionUiModel>> DeleteSoftDepartmentAsync(DeleteSoftDepartmentCommand deleteCommand)
        {
            var bc = new BusinessResult<DepartmentDeletionUiModel>(new DepartmentDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var department = _departmentRepository.FindBy(deleteCommand.Id);
            if (department is null || !department.Active)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "Department Id does not exist"));
                return bc;
            }

            department.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(department, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Successful = true;
            bc.Model.Hard = false;
            bc.Model.Message = $"Department with id: {deleteCommand.Id} has been deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(Department department, long id)
        {
            _departmentRepository.Save(department, id);
            _uOf.Commit();
        }
    }
}
