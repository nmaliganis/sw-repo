using System;
using sw.auth.common.dtos.Vms.Accounts;
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

    public class GetUsersByCompanyQuery : GetUsersResourceParameters, IRequest<BusinessResult<PagedList<UserUiModel>>>
    {
        public long CompanyId { get; }

        public GetUsersByCompanyQuery(long companyId, GetUsersResourceParameters parameters) : base()
        {
            CompanyId = companyId;
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    public class GetUsersByDepartmentQuery : GetUsersResourceParameters, IRequest<BusinessResult<PagedList<UserUiModel>>>
    {
        public long DepartmentId { get; }

        public GetUsersByDepartmentQuery(long departmentId, GetUsersResourceParameters parameters) : base()
        {
            DepartmentId = departmentId;
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands

    public record UpdateUserActivationCommand(long UserIdToBeActivated, long UserAuditToActivate, AccountForActivationModification AccountForActivation)
        : IRequest<BusinessResult<UserActivationUiModel>>;

    public record UpdateUserOnlyWithActivationCommand(AccountForActivationModification AccountForActivation)
        : IRequest<BusinessResult<UserActivationUiModel>>;

    public record UpdateUserStatusCommand(long MemberId, long CompanyId, long UserAuditId, ChangeStatusUiModel ChangedStatus)
        : IRequest<BusinessResult<UserStatusUiModel>>;

    public record UpdateUserWithNewPasswordCommand(long AccountIdToUpdatePassword, long UserIdToBePasswordChanged, ChangePasswordUiModel ChangePasswordUiModel)
        : IRequest<BusinessResult<bool>>;

    public record UpdateUserLogoutCommand(long MemberId, long UserAuditId)
        : IRequest<BusinessResult<bool>>;

    public record CreateUserCommand(long AccountIdToCreateThisUser, UserForRegistrationUiModel UserForRegistrationUiModel)
        : IRequest<BusinessResult<UserUiModel>>;

    public record ForgetMemberPasswordByEmailCommand(string Email)
        : IRequest<BusinessResult<bool>>;

    public record UpdateUserOnlyWithActivationDirectCommand(Guid ActivationCode)
        : IRequest<BusinessResult<UserActivationUiModel>>;

    public record UpdateUserCommand(long ModifiedById, long Id, string Name, string CodeErp, string Description)
        : IRequest<BusinessResult<UserModificationUiModel>>;

    public record UpdateUserWithNewRefreshTokenCommand(UserUiModel RegisteredUser, Guid NewRefreshedToken)
        : IRequest<BusinessResult<UserUiModel>>;

    public record DeleteSoftUserCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<UserDeletionUiModel>>;

    public record DeleteHardUserCommand(long Id)
        : IRequest<BusinessResult<UserDeletionUiModel>>;
}
