using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;

namespace sw.asset.contracts.V1.SimcardProcessors;

public interface IGetSimcardsProcessor
{
  Task<BusinessResult<PagedList<SimcardUiModel>>> GetSimcardsAsync(GetSimcardsQuery qry);
}