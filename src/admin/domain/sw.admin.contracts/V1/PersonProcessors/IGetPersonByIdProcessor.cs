using sw.admin.common.dtos.V1.Vms.Persons;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.PersonProcessors
{
    public interface IGetPersonByIdProcessor
    {
        Task<BusinessResult<PersonUiModel>> GetPersonByIdAsync(long id);
    }
}
