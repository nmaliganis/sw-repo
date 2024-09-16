using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Users;
using sw.auth.model.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;

namespace sw.auth.services.V1.Users
{
    public class UpdateUserProcessor : IUpdateUserProcessor, IRequestHandler<UpdateUserCommand, BusinessResult<UserModificationUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IUserRepository _userRepository;
        private readonly IAutoMapper _autoMapper;
        public UpdateUserProcessor(IUnitOfWork uOf, IAutoMapper autoMapper, IUserRepository userRepository)
        {
            _uOf = uOf;
            _autoMapper = autoMapper;
            _userRepository = userRepository;
        }

        public async Task<BusinessResult<UserModificationUiModel>> Handle(UpdateUserCommand updateCommand, CancellationToken cancellationToken)
        {
            return await UpdateUserAsync(updateCommand);
        }

        public async Task<BusinessResult<UserModificationUiModel>> UpdateUserAsync(UpdateUserCommand updateCommand)
        {
            var bc = new BusinessResult<UserModificationUiModel>(new UserModificationUiModel());

            if (updateCommand.IsNull())
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var user = _userRepository.FindBy(updateCommand.Id);
            if (user.IsNull())
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.Id), "User Id does not exist"));
                return bc;
            }

            //if (refreshToken == Guid.Empty || string.IsNullOrEmpty(registeredUser.Login)) {
            //    response.Message = "ERROR_INVALID_USER_REFRESH_TOKEN_OR_LOGIN";
            //    return Task.Run(() => response);
            //}

            //try {
            //    var userToBeModified = _userRepository.FindUserByLogin(registeredUser.Login);

            //    if (userToBeModified == null)
            //        throw new UserDoesNotExistException(refreshToken);

            //    userToBeModified.InjectWithRefreshToken(new RefreshToken() {
            //        Token = refreshToken
            //    });

            //    Log.Debug(
            //        $"Update User with RefreshToken: {refreshToken}" +
            //        "--UpdateUserRefreshTokenAsync--  @NotComplete@ [UpdateUserProcessor]. " +
            //        "Message: Just Before MakeItPersistence");

            //    MakePatientPersistent(userToBeModified);

            //    Log.Debug(
            //        $"Update User with RefreshToken: {refreshToken}" +
            //        "--UpdateUserRefreshTokenAsync--  @NotComplete@ [UpdateUserProcessor]. " +
            //        "Message: Just After MakeItPersistence");

            //    response.Message = "SUCCESS_CREATION";

            //    return Task.Run(() => _autoMapper.Map<UserRetrievalUiModel>(userToBeModified));
            //} catch (UserDoesNotExistException ex) {
            //    response.Message = "ERROR_USER_DOES_NOT_EXISTS";
            //    Log.Error(
            //        $"Update User with RefreshToken: {refreshToken}" +
            //        $"Error Message:{response.Message}" +
            //        $"--UpdateUserRefreshTokenAsync--  @fail@ [UpdateUserProcessor]. " +
            //        $"@innerfault:{ex.Message} and {ex.InnerException}");
            //} catch (Exception ex) {
            //    response.Message = "UNKNOWN_ERROR";
            //    Log.Error(
            //        $"Update User with RefreshToken: {refreshToken}" +
            //        $"Error Message:{response.Message}" +
            //        $"--UpdateUserRefreshTokenAsync--  @fail@ [UpdateUserProcessor]. " +
            //        $"@innerfault:{ex.Message} and {ex.InnerException}");
            //}

            return await Task.FromResult(bc);
        }

        private void Persist(User user, long id)
        {
            _userRepository.Save(user, id);
            _uOf.Commit();
        }
    }
}