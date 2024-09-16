using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.DepartmentPersonRoles
{
    internal class CreateDepartmentPersonRoleHandler :
        IRequestHandler<CreateDepartmentPersonRoleCommand, BusinessResult<DepartmentPersonRoleCreationUiModel>>
    {
        private readonly ICreateDepartmentPersonRoleProcessor _processor;

        public CreateDepartmentPersonRoleHandler(ICreateDepartmentPersonRoleProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentPersonRoleCreationUiModel>> Handle(CreateDepartmentPersonRoleCommand createCommand, CancellationToken cancellationToken)
        {
            return await _processor.CreateDepartmentPersonRoleAsync(createCommand);
        }
    }
}
