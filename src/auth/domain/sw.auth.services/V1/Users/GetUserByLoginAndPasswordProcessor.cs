using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Users;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.TypeMappings;
using MediatR;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;

namespace sw.auth.services.V1.Users {
    public class GetUserByLoginAndPasswordProcessor : IGetUserByLoginAndPasswordProcessor,
        IRequestHandler<GetUserAuthJwtTokenByLoginAndPasswordQuery, BusinessResult<UserUiModel>> {
        private readonly IAutoMapper _autoMapper;
        private readonly IUserRepository _userRepository;
        public GetUserByLoginAndPasswordProcessor(IUserRepository userRepository, IAutoMapper autoMapper) {
            this._userRepository = userRepository;
            this._autoMapper = autoMapper;
        }

        public async Task<BusinessResult<UserUiModel>> GetUserByLoginAndPasswordAsync(string login, string password) {
            var bc = new BusinessResult<UserUiModel>(new UserUiModel());

            if (login.IsNull()) {
                bc.BrokenRules.Add(BusinessError.CreateInstance("GetUserByLoginAndPasswordAsync", "ERROR_INVALID_USERNAME"));
                return await Task.FromResult(bc);
            }
            if (password.IsNull()) {
                bc.BrokenRules.Add(BusinessError.CreateInstance("GetUserByLoginAndPasswordAsync", "ERROR_INVALID_PASSWORD"));
                return await Task.FromResult(bc);
            }

            try {
                var userAuth = this._userRepository.FindUserByLoginAndPasswordAsync(login, password);
                if (userAuth.IsNull()) {
                    bc.BrokenRules.Add(BusinessError.CreateInstance("GetUserByLoginAndPasswordAsync", "ERROR_NONE_USER_RETRIEVED"));
                    return await Task.FromResult(bc);
                }

                UserUiModel response = new UserUiModel();
                if (!bc.IsSuccess()) {
                    response.Message = "OTHER_ERROR";
                    bc.Model = response;
                    return await Task.FromResult(bc);
                }

                response = this._autoMapper.Map<UserUiModel>(userAuth);
                response.Message = "SUCCESS_RETRIEVAL";

                bc.Model = response;

                return await Task.FromResult(bc);
            } catch (Exception ex) {
                bc.BrokenRules.Add(BusinessError.CreateInstance("GetUserByLoginAndPasswordAsync", "UNKNOWN_ERROR"));
                Log.Error(
                    $"Get User Auth JwtToken: {login}" +
                    $"Error Message:{bc.BrokenRules.FirstOrDefault()?.Rule}" +
                    $"--GetUserAuthJwtToken--  @fail@ [GetUserByLoginAndPasswordAsync]. " +
                    $"@innerfault:{ex.Message} and {ex.InnerException}");
            }


            return await Task.FromResult(bc);
        }

        public async Task<BusinessResult<UserUiModel>> Handle(GetUserAuthJwtTokenByLoginAndPasswordQuery qry, CancellationToken cancellationToken) {
            return await this.GetUserByLoginAndPasswordAsync(qry.Login, qry.Password);
        }
    }
}