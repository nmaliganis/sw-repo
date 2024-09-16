using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Users;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Users
{
    public interface IGetUserByLoginAndPasswordProcessor
    {
        Task<BusinessResult<UserUiModel>> GetUserByLoginAndPasswordAsync(string login , string password);
    }
}