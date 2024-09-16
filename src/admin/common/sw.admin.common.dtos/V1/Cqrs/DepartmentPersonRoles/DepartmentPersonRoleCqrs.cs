using sw.admin.common.dtos.V1.ResourceParameters.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles
{
    // Queries
    public record GetDepartmentPersonRoleByIdQuery(long Id) : IRequest<BusinessResult<DepartmentPersonRoleUiModel>>;

    public class GetDepartmentPersonRolesQuery : GetDepartmentPersonRolesResourceParameters, IRequest<BusinessResult<PagedList<DepartmentPersonRoleUiModel>>>
    {
        public GetDepartmentPersonRolesQuery(GetDepartmentPersonRolesResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            SortDirection = parameters.SortDirection;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateDepartmentPersonRoleCommand(long CreatedById, CreateDepartmentPersonRoleResourceParameters Parameters)
        : IRequest<BusinessResult<DepartmentPersonRoleCreationUiModel>>;

    public record UpdateDepartmentPersonRoleCommand(long Id, long ModifiedById, UpdateDepartmentPersonRoleResourceParameters Parameters)
        : IRequest<BusinessResult<DepartmentPersonRoleModificationUiModel>>;

    public record DeleteSoftDepartmentPersonRoleCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<DepartmentPersonRoleDeletionUiModel>>;

    public record DeleteHardDepartmentPersonRoleCommand(long Id)
        : IRequest<BusinessResult<DepartmentPersonRoleDeletionUiModel>>;
}
