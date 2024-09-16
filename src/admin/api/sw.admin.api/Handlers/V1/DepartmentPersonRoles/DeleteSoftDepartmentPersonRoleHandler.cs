using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.DepartmentPersonRoles
{
    internal class DeleteSoftDepartmentPersonRoleHandler :
        IRequestHandler<DeleteSoftDepartmentPersonRoleCommand, BusinessResult<DepartmentPersonRoleDeletionUiModel>>
    {
        private readonly IDeleteSoftDepartmentPersonRoleProcessor _processor;

        public DeleteSoftDepartmentPersonRoleHandler(IDeleteSoftDepartmentPersonRoleProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentPersonRoleDeletionUiModel>> Handle(DeleteSoftDepartmentPersonRoleCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await _processor.DeleteSoftDepartmentPersonRoleAsync(deleteCommand);
        }
    }
}
