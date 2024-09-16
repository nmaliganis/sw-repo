using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Departments
{
    internal class DeleteHardDepartmentHandler :
        IRequestHandler<DeleteHardDepartmentCommand, BusinessResult<DepartmentDeletionUiModel>>
    {
        private readonly IDeleteHardDepartmentProcessor _processor;

        public DeleteHardDepartmentHandler(IDeleteHardDepartmentProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentDeletionUiModel>> Handle(DeleteHardDepartmentCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await _processor.DeleteHardDepartmentAsync(deleteCommand);
        }
    }
}
