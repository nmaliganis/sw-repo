using sw.admin.common.dtos.V1.Cqrs.Persons;
using sw.admin.common.dtos.V1.Vms.Persons;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.admin.contracts.V1.PersonProcessors
{
    public interface IGetPersonsProcessor
    {
        Task<BusinessResult<PagedList<PersonUiModel>>> GetPersonsAsync(GetPersonsQuery qry);
    }
}
