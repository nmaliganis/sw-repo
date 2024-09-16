using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.ResourceParameters.Templates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.routing.common.dtos.Cqrs.ItineraryTemplates
{
    // Queries
    public record GetItineraryTemplateByIdQuery(long Id) : IRequest<BusinessResult<ItineraryTemplateUiModel>>;

    public class GetItineraryTemplatesQuery : GetItinerariesResourceParameters, IRequest<BusinessResult<PagedList<ItineraryTemplateUiModel>>>
    {
        public GetItineraryTemplatesQuery(GetItinerariesResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateItineraryTemplateCommand(long CreatedById, CreateItineraryTemplateResourceParameters Parameters)
        : IRequest<BusinessResult<ItineraryTemplateUiModel>>;

    public record UpdateItineraryTemplateCommand(long ModifiedById, long Id, UpdateItineraryTemplateResourceParameters Parameters)
        : IRequest<BusinessResult<ItineraryTemplateUiModel>>;

    public record DeleteSoftItineraryTemplateCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<ItineraryTemplateDeletionUiModel>>;

    public record DeleteHardItineraryTemplateCommand(long Id)
        : IRequest<BusinessResult<ItineraryTemplateDeletionUiModel>>;
}
