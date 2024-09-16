using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.admin.api.Handlers.V1.DepartmentPersonRoles
{
    internal class GetDepartmentPersonRoleByIdHandler :
        IRequestHandler<GetDepartmentPersonRoleByIdQuery, BusinessResult<DepartmentPersonRoleUiModel>>
    {
        private readonly IGetDepartmentPersonRoleByIdProcessor _processor;

        public GetDepartmentPersonRoleByIdHandler(IGetDepartmentPersonRoleByIdProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentPersonRoleUiModel>> Handle(GetDepartmentPersonRoleByIdQuery qry, CancellationToken cancellationToken)
        {
            return await _processor.GetDepartmentPersonRoleByIdAsync(qry.Id);
        }
    }
}
