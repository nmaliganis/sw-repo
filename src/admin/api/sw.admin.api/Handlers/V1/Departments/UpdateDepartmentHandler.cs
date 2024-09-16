using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Departments
{
    internal class UpdateDepartmentHandler :
        IRequestHandler<UpdateDepartmentCommand, BusinessResult<DepartmentModificationUiModel>>
    {
        private readonly IUpdateDepartmentProcessor _processor;

        public UpdateDepartmentHandler(IUpdateDepartmentProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentModificationUiModel>> Handle(UpdateDepartmentCommand updateCommand, CancellationToken cancellationToken)
        {
            return await _processor.UpdateDepartmentAsync(updateCommand);
        }
    }
}
