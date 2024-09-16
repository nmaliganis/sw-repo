using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Departments;
using sw.auth.common.dtos.Vms.Departments;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.DepartmentProcessors;
using sw.auth.model.Departments;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;

namespace sw.auth.services.V1.Departments
{
    public class DeleteSoftDepartmentProcessor :
        IDeleteSoftDepartmentProcessor,
        IRequestHandler<DeleteSoftDepartmentCommand, BusinessResult<DepartmentDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IDepartmentRepository _departmentRepository;

        public DeleteSoftDepartmentProcessor(IUnitOfWork uOf, IDepartmentRepository departmentRepository)
        {
            _uOf = uOf;
            _departmentRepository = departmentRepository;
        }

        public async Task<BusinessResult<DepartmentDeletionUiModel>> Handle(DeleteSoftDepartmentCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteSoftDepartmentAsync(deleteCommand);
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

            department.DeleteWithAudit(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

            Persist(department, deleteCommand.Id);

            bc.Model.Id = deleteCommand.Id;
            bc.Model.Active = false;
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
