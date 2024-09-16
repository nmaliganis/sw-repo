using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.SimcardProcessors;

public interface IDeleteSoftSimcardProcessor
{
  Task<BusinessResult<SimcardDeletionUiModel>> DeleteSoftSimcardAsync(DeleteSoftSimcardCommand deleteCommand);
}