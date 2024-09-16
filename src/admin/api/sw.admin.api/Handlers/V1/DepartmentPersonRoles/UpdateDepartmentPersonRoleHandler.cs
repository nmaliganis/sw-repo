using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.DepartmentPersonRoles
{
    internal class UpdateDepartmentPersonRoleHandler :
        IRequestHandler<UpdateDepartmentPersonRoleCommand, BusinessResult<DepartmentPersonRoleModificationUiModel>>
    {
        private readonly IUpdateDepartmentPersonRoleProcessor _processor;

        public UpdateDepartmentPersonRoleHandler(IUpdateDepartmentPersonRoleProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentPersonRoleModificationUiModel>> Handle(UpdateDepartmentPersonRoleCommand updateCommand, CancellationToken cancellationToken)
        {
            return await _processor.UpdateDepartmentPersonRoleAsync(updateCommand);
        }
    }
}
