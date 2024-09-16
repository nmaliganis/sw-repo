using sw.asset.common.dtos.Vms.Simcards;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.asset.contracts.V1.SimcardProcessors;

public interface IGetSimcardByIdProcessor
{
  Task<BusinessResult<SimcardUiModel>> GetSimcardByIdAsync(long id);
}