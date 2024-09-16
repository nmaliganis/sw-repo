using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;

namespace sw.auth.services.V1.Users {
    public class GetUserByEmailProcessor : IGetUserByEmailProcessor,
        IRequestHandler<GetUserByEmailQuery, BusinessResult<UserUiModel>> {
        private readonly IAutoMapper _autoMapper;
        private readonly IUserRepository _userRepository;
        public GetUserByEmailProcessor(IUserRepository userRepository, IAutoMapper autoMapper) {
            this._userRepository = userRepository;
            this._autoMapper = autoMapper;
        }

        public async Task<BusinessResult<UserUiModel>> GetUserByEmailAsync(string email) {
            var bc = new BusinessResult<UserUiModel>(new UserUiModel());

            var user = this._userRepository.FindUserByEmail(email);
            if (user.IsNull()) {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(email), "ERROR_INVALID_USER_EMAIL"));
                return bc;
            }

            var response = this._autoMapper.Map<UserUiModel>(user);
            response.Message = "SUCCESS_RETRIEVAL";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<UserUiModel>> Handle(GetUserByEmailQuery qry, CancellationToken cancellationToken) {
            return await this.GetUserByEmailAsync(qry.Email);
        }
    }
}