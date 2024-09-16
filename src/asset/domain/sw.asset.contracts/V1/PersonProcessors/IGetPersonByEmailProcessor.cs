using System.Threading.Tasks;
using sw.asset.common.dtos.Vms.Persons;
using sw.infrastructure.BrokenRules;

namespace sw.asset.contracts.V1.PersonProcessors;

public interface IGetPersonByEmailProcessor
{
  Task<BusinessResult<PersonUiModel>> GetPersonByEmailAsync(string email);
}