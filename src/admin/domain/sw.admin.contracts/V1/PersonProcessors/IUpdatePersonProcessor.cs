using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.PersonProcessors
{
    public interface IUpdatePersonProcessor
    {
        Task<BusinessResult<PersonModificationUiModel>> UpdatePersonAsync(UpdatePersonCommand updateCommand);
    }
}
