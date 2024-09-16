using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.DepartmentPersonRoles
{
    internal class GetDepartmentPersonRolesHandler :
        IRequestHandler<GetDepartmentPersonRolesQuery, BusinessResult<PagedList<DepartmentPersonRoleUiModel>>>
    {
        private readonly IGetDepartmentPersonRolesProcessor _processor;

        public GetDepartmentPersonRolesHandler(IGetDepartmentPersonRolesProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PagedList<DepartmentPersonRoleUiModel>>> Handle(GetDepartmentPersonRolesQuery qry, CancellationToken cancellationToken)
        {
            return await _processor.GetDepartmentPersonRolesAsync(qry);
        }
    }
}
