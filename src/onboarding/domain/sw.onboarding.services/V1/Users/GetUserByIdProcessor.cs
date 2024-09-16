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
    public class GetUserByIdProcessor : IGetUserByIdProcessor,
        IRequestHandler<GetUserByIdQuery, BusinessResult<UserUiModel>> {
        private readonly IAutoMapper _autoMapper;
        private readonly IUserRepository _userRepository;
        public GetUserByIdProcessor(IUserRepository userRepository, IAutoMapper autoMapper) {
            this._userRepository = userRepository;
            this._autoMapper = autoMapper;
        }

        public async Task<BusinessResult<UserUiModel>> GetUserByIdAsync(long id) {
            var bc = new BusinessResult<UserUiModel>(new UserUiModel());

            var user = this._userRepository.FindBy(id);
            if (user.IsNull()) {
                bc.AddBrokenRule(BusinessError.CreateInstance(nameof(id), "ERROR_INVALID_USER_ID"));
                return bc;
            }

            var response = this._autoMapper.Map<UserUiModel>(user);
            response.Message = "SUCCESS_RETRIEVAL";

            bc.Model = response;

            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<UserUiModel>> Handle(GetUserByIdQuery qry, CancellationToken cancellationToken) {
            return await this.GetUserByIdAsync(qry.Id);
        }
    }
}
