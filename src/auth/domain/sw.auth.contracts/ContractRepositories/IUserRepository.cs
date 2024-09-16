using sw.auth.model.Users;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;
using System;

namespace sw.auth.contracts.ContractRepositories;

public interface IUserRepository : IRepository<User, long>
{
    QueryResult<User> FindAllActiveUsersPagedOf(int? pageNum, int? pageSize);
    QueryResult<User> FindAllActiveUsersByCompanyPagedOf(long companyId, int? pageNum, int? pageSize);
    QueryResult<User> FindAllActiveUsersByDepartmentPagedOf(long departmentId, int? pageNum, int? pageSize);

    User FindUserByLoginAndEmail(string login, string email);
    User FindUserByLogin(string login);
    User FindUserByEmail(string email);

    User FindUserByLoginAndPasswordAsync(string login, string password);

    User FindByUserIdAndActivationKey(long userIdToBeActivated, Guid activationKey);
    User FindUserByRefreshTokenAsync(Guid refreshToken);
}