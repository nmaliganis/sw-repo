using sw.routing.common.dtos.ResourceParameters.Itineraries;
using sw.routing.common.dtos.Vms.SpecialDays;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.routing.common.dtos.Cqrs.SpecialDays
{
    // Queries
    public record GetSpecialDayByIdQuery(long Id) : IRequest<BusinessResult<SpecialDayUiModel>>;

    public class GetSpecialDaysQuery : GetItinerariesResourceParameters, IRequest<BusinessResult<PagedList<SpecialDayUiModel>>>
    {
        public GetSpecialDaysQuery(GetItinerariesResourceParameters parameters) : base()
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
    public record CreateSpecialDayCommand(long CreatedById, string Name)
        : IRequest<BusinessResult<SpecialDayUiModel>>;

    public record UpdateSpecialDayCommand(long ModifiedById, long Id, string Name)
        : IRequest<BusinessResult<SpecialDayModificationUiModel>>;

    public record DeleteSoftSpecialDayCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<SpecialDayDeletionUiModel>>;

    public record DeleteHardSpecialDayCommand(long Id)
        : IRequest<BusinessResult<SpecialDayDeletionUiModel>>;
}
