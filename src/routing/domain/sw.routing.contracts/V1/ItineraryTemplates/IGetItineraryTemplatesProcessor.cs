using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;

namespace sw.routing.contracts.V1.ItineraryTemplates;

public interface IGetItineraryTemplatesProcessor
{
    Task<BusinessResult<PagedList<ItineraryTemplateUiModel>>> GetItineraryTemplatesAsync(GetItineraryTemplatesQuery qry);
}