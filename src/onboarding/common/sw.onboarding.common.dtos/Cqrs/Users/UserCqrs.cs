using System;
using sw.auth.common.dtos.Vms.Users;
using sw.common.dtos.ResourceParameters.Users;
using sw.common.dtos.Vms.Accounts;
using sw.common.dtos.Vms.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.auth.common.dtos.Cqrs.Users
{
    // Queries
    public record GetUserByIdQuery(long Id) : IRequest<BusinessResult<UserUiModel>>;
    public record GetUserByEmailQuery(string Email) : IRequest<BusinessResult<UserUiModel>>;
    public record GetUserAuthJwtTokenByLoginAndPasswordQuery(string Login, string Password) : IRequest<BusinessResult<UserUiModel>>;

    public class GetUsersQuery : GetUsersResourceParameters, IRequest<BusinessResult<PagedList<UserUiModel>>>
    {
        public GetUsersQuery(GetUsersResourceParameters parameters) : base()
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
    public record CreateUserCommand(long AccountIdToCreateThisUser, UserForRegistrationUiModel UserForRegistrationUiModel)
        : IRequest<BusinessResult<UserUiModel>>;

    public record UpdateUserCommand(long ModifiedById, long Id, string Name, string CodeErp, string Description)
        : IRequest<BusinessResult<UserModificationUiModel>>;

    public record UpdateUserWithNewRefreshTokenCommand(UserUiModel RegisteredUser, Guid NewRefreshedToken)
        : IRequest<BusinessResult<UserUiModel>>;

    public record DeleteSoftUserCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<UserDeletionUiModel>>;

    public record DeleteHardUserCommand(long Id)
        : IRequest<BusinessResult<UserDeletionUiModel>>;
}
