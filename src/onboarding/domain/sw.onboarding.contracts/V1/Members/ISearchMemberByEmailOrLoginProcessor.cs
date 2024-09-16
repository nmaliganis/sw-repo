using System.Threading.Tasks;
using sw.infrastructure.BrokenRules;

namespace sw.auth.contracts.V1.Members
{
    public interface ISearchMemberByEmailOrLoginProcessor
    {
        Task<BusinessResult<bool>> SearchIfAnyMemberByEmailOrLoginExistsAsync(string email, string login);
    }
}
