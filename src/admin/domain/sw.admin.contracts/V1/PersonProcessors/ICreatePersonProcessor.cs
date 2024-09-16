using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.PersonProcessors
{
    public interface ICreatePersonProcessor
    {
        Task<BusinessResult<PersonCreationUiModel>> CreatePersonAsync(CreatePersonCommand createCommand);
    }
}
