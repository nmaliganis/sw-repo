using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Departments
{
    internal class DeleteSoftDepartmentHandler :
        IRequestHandler<DeleteSoftDepartmentCommand, BusinessResult<DepartmentDeletionUiModel>>
    {
        private readonly IDeleteSoftDepartmentProcessor _processor;

        public DeleteSoftDepartmentHandler(IDeleteSoftDepartmentProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentDeletionUiModel>> Handle(DeleteSoftDepartmentCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await _processor.DeleteSoftDepartmentAsync(deleteCommand);
        }
    }
}
