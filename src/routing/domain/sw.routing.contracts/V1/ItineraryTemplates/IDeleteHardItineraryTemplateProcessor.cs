using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.infrastructure.BrokenRules;

namespace sw.routing.contracts.V1.ItineraryTemplates
{
    public interface IDeleteHardItineraryTemplateProcessor
    {
        Task<BusinessResult<ItineraryTemplateDeletionUiModel>> DeleteHardItineraryTemplateAsync(DeleteHardItineraryTemplateCommand deleteCommand);
    }
}
