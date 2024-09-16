using sw.asset.common.dtos.Cqrs.Persons;
using sw.asset.common.dtos.Vms.Persons;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.PersonProcessors;

public interface IGetPersonsProcessor
{
  Task<BusinessResult<PagedList<PersonUiModel>>> GetPersonsAsync(GetPersonsQuery qry);
}