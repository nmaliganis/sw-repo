using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.Departments
{
    internal class CreateDepartmentHandler :
        IRequestHandler<CreateDepartmentCommand, BusinessResult<DepartmentCreationUiModel>>
    {
        private readonly ICreateDepartmentProcessor _processor;

        public CreateDepartmentHandler(ICreateDepartmentProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentCreationUiModel>> Handle(CreateDepartmentCommand createCommand, CancellationToken cancellationToken)
        {
            return await _processor.CreateDepartmentAsync(createCommand);
        }
    }
}
