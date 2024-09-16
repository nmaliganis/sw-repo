using sw.auth.common.infrastructure.Exceptions.Users;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Users;
using sw.auth.model.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;

namespace sw.auth.services.V1.Users;

public class UpdateUserWithRefreshTokenProcessor : IUpdateUserWithRefreshTokenProcessor,
  IRequestHandler<UpdateUserWithNewRefreshTokenCommand, BusinessResult<UserUiModel>> {
  private readonly IUnitOfWork _uOf;
  private readonly IUserRepository _userRepository;
  private readonly IAutoMapper _autoMapper;
  public UpdateUserWithRefreshTokenProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
    IUserRepository userRepository) {
    this._uOf = uOf;
    this._autoMapper = autoMapper;
    this._userRepository = userRepository;
  }

  private void Persist(User user, long id) {
    this._userRepository.Save(user, id);
    this._uOf.Commit();
  }

  public async Task<BusinessResult<UserUiModel>> Handle(UpdateUserWithNewRefreshTokenCommand updateCommand, CancellationToken cancellationToken) {
    return await this.UpdateUserAsync(updateCommand);
  }

  public async Task<BusinessResult<UserUiModel>> UpdateUserAsync(UpdateUserWithNewRefreshTokenCommand updateCommand) {
    var bc = new BusinessResult<UserUiModel>(new UserUiModel());

    if (updateCommand is null) {
      bc.AddBrokenRule(new BusinessError(null));
    }

    if (updateCommand != null) {
      var user = this._userRepository.FindBy(updateCommand.RegisteredUser.Id);
      if (user is null) {
        bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.RegisteredUser), "ERROR_INVALID_USER_ID"));
        return bc;
      }
    }

    if (updateCommand != null && (updateCommand.NewRefreshedToken == Guid.Empty || string.IsNullOrEmpty(updateCommand.RegisteredUser.Login))) {
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.RegisteredUser), "ERROR_INVALID_USER_REFRESH_TOKEN_OR_LOGIN"));
      return bc;
    }

    try {
      if (updateCommand != null)
      {
        var userToBeModified = this._userRepository.FindUserByLogin(updateCommand.RegisteredUser.Login);

        if (userToBeModified == null)
        {
          throw new UserDoesNotExistException(updateCommand.RegisteredUser.Id);
        }

        userToBeModified.InjectWithRefreshToken(new RefreshToken()
        {
          JwtToken = updateCommand.NewRefreshedToken
        });

        userToBeModified.ModifyWithAudit(updateCommand.RegisteredUser.Id);

        Log.Debug(
          $"Update User with RefreshToken: {updateCommand.NewRefreshedToken}" +
          "--UpdateUserRefreshTokenAsync--  @NotComplete@ [UpdateUserProcessor]. " +
          "Message: Just Before MakeItPersistence");

        this.MakeUserPersistent(userToBeModified);


        Log.Debug(
          $"Update User with RefreshToken: {updateCommand?.NewRefreshedToken}" +
          "--UpdateUserRefreshTokenAsync--  @NotComplete@ [UpdateUserProcessor]. " +
          "Message: Just After MakeItPersistence");

        Log.Warning(
          $"Update User with RefreshToken: {updateCommand?.NewRefreshedToken}" +
          "--UpdateUserRefreshTokenAsync--  @NotComplete@ [UpdateUserProcessor]. " +
          "Message: SUCCESS_UPDATE");

        bc.Model = ThrowExcIfUserWasNotBeMadePersistent(userToBeModified);
      }
    } catch (UserDoesNotExistException ex) {
      string errorMessage = "ERROR_USER_DOES_NOT_EXISTS";
      bc.AddBrokenRule(BusinessError.CreateInstance(nameof(updateCommand.RegisteredUser), errorMessage));
      if (updateCommand != null) {
        Log.Error(
          $"Update User with RefreshToken: {updateCommand.NewRefreshedToken}" +
          $"Error Message:{errorMessage}" +
          $"--UpdateUserRefreshTokenAsync--  @fail@ [UpdateUserProcessor]. " +
          $"@innerfault:{ex.Message} and {ex.InnerException}");
      }
    } catch (Exception ex) {
      string errorMessage = "UNKNOWN_ERROR";
      if (updateCommand != null) {
        Log.Error(
          $"Update User with RefreshToken: {updateCommand.NewRefreshedToken}" +
          $"Error Message:{errorMessage}" +
          $"--UpdateUserRefreshTokenAsync--  @fail@ [UpdateUserProcessor]. " +
          $"@innerfault:{ex.Message} and {ex.InnerException}");
      }
    }

    return await Task.FromResult(bc);
  }

  private void MakeUserPersistent(User userToBeMadePersistence) {
    this._userRepository.Save(userToBeMadePersistence);
    this._uOf.Commit();
  }

  private UserUiModel ThrowExcIfUserWasNotBeMadePersistent(User userToBeCreated) {
    var retrievedUser =
      this._userRepository.FindUserByLoginAndEmail(userToBeCreated.Login, userToBeCreated.Member?.Email);
    if (retrievedUser != null) {
      return this._autoMapper.Map<UserUiModel>(retrievedUser);
    }

    throw new UserDoesNotExistAfterMadePersistentException(userToBeCreated.Login, userToBeCreated.Member?.Email);
  }
}//Class : UpdateUserWithRefreshTokenProcessor