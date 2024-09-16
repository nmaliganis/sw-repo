using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;
using sw.auth.common.infrastructure.Exceptions.Roles;
using sw.auth.common.infrastructure.Exceptions.Users;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Users;
using sw.auth.model.Users;
using sw.common.dtos.Vms.Accounts;
using sw.onboarding.model.Members;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.TypeMappings;
using sw.infrastructure.UnitOfWorks;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sw.onboarding.services.V1.Users;

public class CreateUserProcessor : ICreateUserProcessor,
  IRequestHandler<CreateUserCommand, BusinessResult<UserUiModel>>
{
    private readonly IUnitOfWork _uOf;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IAutoMapper _autoMapper;

    public CreateUserProcessor(IUnitOfWork uOf, IAutoMapper autoMapper,
      IUserRepository userRepository, IRoleRepository roleRepository, IDepartmentRepository departmentRepository)
    {
        _uOf = uOf;
        _autoMapper = autoMapper;
        this._userRepository = userRepository;
        this._roleRepository = roleRepository;
        this._departmentRepository = departmentRepository;
    }

    private void MakeUserPersistent(User userToBeMadePersistence)
    {
        _userRepository.Save(userToBeMadePersistence);
        _uOf.Commit();
    }

    private UserUiModel ThrowExcIfUserWasNotBeMadePersistent(User userToBeCreated)
    {
        var retrievedUser =
          _userRepository.FindUserByLoginAndEmail(userToBeCreated.Login, userToBeCreated.Member?.Email);
        if (retrievedUser != null)
            return _autoMapper.Map<UserUiModel>(retrievedUser);
        throw new UserDoesNotExistAfterMadePersistentException(userToBeCreated.Login, userToBeCreated.Member?.Email);
    }

    private void ThrowExcIfUserCannotBeCreated(User userToBeCreated)
    {
        bool canBeCreated = !userToBeCreated.GetBrokenRules().Any();
        if (!canBeCreated)
            throw new InvalidUserException(userToBeCreated.GetBrokenRulesAsString());
    }

    private void ThrowExcIfThisUserAlreadyExist(User userToBeCreated)
    {
        var userRetrieved = _userRepository.FindUserByLoginAndEmail(userToBeCreated.Login, userToBeCreated.Member?.Email);
        if (userRetrieved != null)
        {
            throw new UserEmailOrLoginAlreadyExistsException(userToBeCreated.Login, userToBeCreated.Member?.Email,
              userToBeCreated.GetBrokenRulesAsString());
        }
    }

    public async Task<BusinessResult<UserUiModel>> CreateUserAsync(long accountIdToCreateThisUser,
      UserForRegistrationUiModel newUserForRegistration)
    {
        var bc = new BusinessResult<UserUiModel>(new UserUiModel());

        try
        {
            var userToBeCreated = new User();
            userToBeCreated.InjectWithInitialAttributes(newUserForRegistration);
            userToBeCreated.InjectWithAuditCreation(accountIdToCreateThisUser);

            //foreach (var roleId in newUserForRegistration.RoleIds)
            //{
            //    var roleToBeInjected = _roleRepository.FindBy(roleId);

            //    if (roleToBeInjected == null)
            //        throw new RoleDoesNotExistException(roleId);

            //    var userRoleToBeInjected = new UserRole();

            //    userRoleToBeInjected.InjectWithRole(roleToBeInjected);
            //    userRoleToBeInjected.InjectWithAuditCreation(accountIdToCreateThisUser);

            //    userToBeCreated.InjectWithUserRole(userRoleToBeInjected);
            //}

            var memberToBeInjected = new Member();
            memberToBeInjected.InjectWithInitialAttributes(newUserForRegistration);
            memberToBeInjected.InjectWithAuditCreation(accountIdToCreateThisUser);

            userToBeCreated.InjectWithMember(memberToBeInjected);

            ThrowExcIfUserCannotBeCreated(userToBeCreated);
            ThrowExcIfThisUserAlreadyExist(userToBeCreated);

            Log.Debug(
              $"Create User: {newUserForRegistration.Login}" +
              "--CreateUser--  @NotComplete@ [CreateUserProcessor]. " +
              "Message: Just Before MakeItPersistence");

            MakeUserPersistent(userToBeCreated);

            Log.Debug(
              $"Create User: {newUserForRegistration.Login}" +
              "--CreateUser--  @NotComplete@ [CreateUserProcessor]. " +
              "Message: Just After MakeItPersistence");

            bc.Model = ThrowExcIfUserWasNotBeMadePersistent(userToBeCreated);

            Log.Warning(
              $"Create User: {newUserForRegistration.Login}" +
              "--CreateUser--  @NotComplete@ [CreateUserProcessor]. " +
              "Message: SUCCESS_CREATION");

            //Todo: Notify other Microservice for this action
        }
        catch (RoleDoesNotExistException e)
        {
            string errorMessage = "ERROR_INVALID_ROLE_NAME";
            bc.BrokenRules.Add(BusinessError.CreateInstance("CreateUserAsync", errorMessage));
            Log.Error(
              $"Create User: {newUserForRegistration.Login}" +
              $"Error Message:{errorMessage}" +
              "--CreateUser--  @NotComplete@ [CreateUserProcessor]. " +
              $"Broken rules: {e.Message}");
        }
        catch (InvalidUserException e)
        {
            string errorMessage = "ERROR_INVALID_USER_MODEL";

            bc.BrokenRules.Add(BusinessError.CreateInstance("CreateUserAsync", errorMessage));
            Log.Error(
              $"Create User: {newUserForRegistration.Login}" +
              $"Error Message:{errorMessage}" +
              "--CreateUser--  @NotComplete@ [CreateUserProcessor]. " +
              $"Broken rules: {e.BrokenRules}");
        }
        catch (UserDoesNotExistAfterMadePersistentException exx)
        {
            string errorMessage = "ERROR_USER_NOT_MADE_PERSISTENT";
            bc.BrokenRules.Add(BusinessError.CreateInstance("CreateUserAsync", errorMessage));
            Log.Error(
              $"Create User: {newUserForRegistration.Login}" +
              $"Error Message:{errorMessage}" +
              "--CreateUser--  @fail@ [CreateUserProcessor]." +
              $" @inner-fault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (UserEmailOrLoginAlreadyExistsException exx)
        {
            string errorMessage = "ERROR_USER_ALREADY_EXISTS";
            bc.BrokenRules.Add(BusinessError.CreateInstance("CreateUserAsync", errorMessage));
            Log.Error(
              $"Create User: {newUserForRegistration.Login}" +
              $"Error Message:{errorMessage}" +
              "--CreateUser--  @fail@ [CreateUserProcessor]." +
              $" @inner-fault:{exx?.Message} and {exx?.InnerException}");
        }
        catch (Exception exxx)
        {
            string errorMessage = "UNKNOWN_ERROR";
            bc.BrokenRules.Add(BusinessError.CreateInstance("CreateUserAsync", errorMessage));
            Log.Error(
              $"Create User: {newUserForRegistration.Login}" +
              $"Error Message:{errorMessage}" +
              $"--CreateUser--  @fail@ [CreateUserProcessor]. " +
              $"@inner-fault:{exxx.Message} and {exxx.InnerException}");
        }

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<UserUiModel>> Handle(CreateUserCommand cmd, CancellationToken cancellationToken)
    {
        return await CreateUserAsync(cmd.AccountIdToCreateThisUser, cmd.UserForRegistrationUiModel);
    }
}// Class : CreateUserProcessor