using sw.auth.common.dtos.Vms.Roles;
using sw.common.dtos.ResourceParameters.Roles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.onboarding.common.dtos.Cqrs.Roles
{
    // Queries
    public record GetRoleByIdQuery(long Id) : IRequest<BusinessResult<RoleUiModel>>;

    public class GetRolesQuery : GetRolesResourceParameters, IRequest<BusinessResult<PagedList<RoleUiModel>>>
    {
        public GetRolesQuery(GetRolesResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateRoleCommand(long CreatedById, string Name)
        : IRequest<BusinessResult<RoleUiModel>>;

    public record UpdateRoleCommand(long ModifiedById, long Id, string Name)
        : IRequest<BusinessResult<RoleModificationUiModel>>;

    public record DeleteSoftRoleCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<RoleDeletionUiModel>>;

    public record DeleteHardRoleCommand(long Id)
        : IRequest<BusinessResult<RoleDeletionUiModel>>;
}
