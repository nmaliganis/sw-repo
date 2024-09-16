using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.infrastructure.BrokenRules;

namespace sw.asset.contracts.V1.SimcardProcessors;

public interface ICreateSimcardProcessor
{
  Task<BusinessResult<SimcardCreationUiModel>> CreateSimcardAsync(CreateSimcardCommand createCommand);
}