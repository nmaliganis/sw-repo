using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.DepartmentPersonRoles
{
    internal class DeleteHardDepartmentPersonRoleHandler :
        IRequestHandler<DeleteHardDepartmentPersonRoleCommand, BusinessResult<DepartmentPersonRoleDeletionUiModel>>
    {
        private readonly IDeleteHardDepartmentPersonRoleProcessor _processor;

        public DeleteHardDepartmentPersonRoleHandler(IDeleteHardDepartmentPersonRoleProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentPersonRoleDeletionUiModel>> Handle(DeleteHardDepartmentPersonRoleCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await _processor.DeleteHardDepartmentPersonRoleAsync(deleteCommand);
        }
    }
}
