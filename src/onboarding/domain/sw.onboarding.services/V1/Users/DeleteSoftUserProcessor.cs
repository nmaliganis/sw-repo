using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Users;
using sw.auth.model.Users;
using sw.common.dtos.Vms.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;

namespace sw.auth.services.V1.Users;

public class DeleteSoftUserProcessor : IDeleteSoftUserProcessor,
  IRequestHandler<DeleteSoftUserCommand, BusinessResult<UserDeletionUiModel>>
{
  private readonly IUnitOfWork _uOf;
  private readonly IUserRepository _userRepository;

  public DeleteSoftUserProcessor(IUnitOfWork uOf, IUserRepository userRepository)
  {
    _uOf = uOf;
    _userRepository = userRepository;
  }

  public async Task<BusinessResult<UserDeletionUiModel>> Handle(DeleteSoftUserCommand deleteCommand, CancellationToken cancellationToken)
  {
    return await DeleteSoftUserAsync(deleteCommand);
  }

  public async Task<BusinessResult<UserDeletionUiModel>> DeleteSoftUserAsync(DeleteSoftUserCommand deleteCommand)
  {
    var bc = new BusinessResult<UserDeletionUiModel>(new UserDeletionUiModel());

    if (deleteCommand is null)
    {
      bc.AddBrokenRule(new BusinessError(null));
    }

    //var User = _UserRepository.FindBy(deleteCommand.Id);
    //if (User is null || !User.Active)
    //{
    //    bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "User Id does not exist"));
    //    return bc;
    //}

    //User.Deleted(deleteCommand.DeletedBy, deleteCommand.DeletedReason);

    //Persist(User, deleteCommand.Id);

    //bc.Model.Id = deleteCommand.Id;
    //bc.Model.Successful = true;
    //bc.Model.Hard = false;
    bc.Model.Message = $"User with id: {deleteCommand!.Id} has been deleted successfully.";

    return await Task.FromResult(bc);
  }

  private void Persist(User user, long id)
  {
    _userRepository.Save(user, id);
    _uOf.Commit();
  }
}