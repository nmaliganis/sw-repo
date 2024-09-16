using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Users;
using sw.auth.model.Users;
using sw.common.dtos.Vms.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.UnitOfWorks;
using MediatR;

namespace sw.auth.services.V1.Users
{
    public class DeleteHardUserProcessor : IDeleteHardUserProcessor,
        IRequestHandler<DeleteHardUserCommand, BusinessResult<UserDeletionUiModel>>
    {
        private readonly IUnitOfWork _uOf;
        private readonly IUserRepository _userRepository;

        public DeleteHardUserProcessor(IUnitOfWork uOf, IUserRepository userRepository)
        {
            _uOf = uOf;
            _userRepository = userRepository;
        }

        public async Task<BusinessResult<UserDeletionUiModel>> Handle(DeleteHardUserCommand deleteCommand, CancellationToken cancellationToken)
        {
            return await DeleteHardUserAsync(deleteCommand);
        }

        public async Task<BusinessResult<UserDeletionUiModel>> DeleteHardUserAsync(DeleteHardUserCommand deleteCommand)
        {
            var bc = new BusinessResult<UserDeletionUiModel>(new UserDeletionUiModel());

            if (deleteCommand is null)
            {
                bc.AddBrokenRule(new BusinessError(null));
            }

            var user = _userRepository.FindBy(deleteCommand!.Id);
            if (user is null)
            {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(deleteCommand.Id), "User Id does not exist"));
                return bc;
            }

            Persist(user);

            //bc.Model.Id = deleteCommand.Id;
            //bc.Model.Successful = true;
            //bc.Model.Hard = true;
            //bc.Model.Message = $"User with id: {deleteCommand.Id} has been hard deleted successfully.";

            return await Task.FromResult(bc);
        }

        private void Persist(User user)
        {
            _userRepository.Remove(user);
            _uOf.Commit();
        }
    }
}
