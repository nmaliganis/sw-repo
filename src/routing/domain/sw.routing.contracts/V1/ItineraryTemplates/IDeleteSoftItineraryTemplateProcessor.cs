using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.infrastructure.BrokenRules;

namespace sw.routing.contracts.V1.ItineraryTemplates;

public interface IDeleteSoftItineraryTemplateProcessor
{
    Task<BusinessResult<ItineraryTemplateDeletionUiModel>> DeleteSoftItineraryTemplateAsync(DeleteSoftItineraryTemplateCommand deleteCommand);
}