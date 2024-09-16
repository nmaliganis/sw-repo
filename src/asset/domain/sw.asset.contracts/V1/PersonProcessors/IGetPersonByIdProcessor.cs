using sw.asset.common.dtos.Vms.Persons;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.PersonProcessors;

public interface IGetPersonByIdProcessor
{
  Task<BusinessResult<PersonUiModel>> GetPersonByIdAsync(long id);
}