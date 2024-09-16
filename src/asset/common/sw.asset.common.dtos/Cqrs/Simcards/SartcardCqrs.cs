using sw.asset.common.dtos.ResourceParameters.Simcards;
using sw.asset.common.dtos.Vms.Simcards;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.common.dtos.Cqrs.Simcards
{
    // Queries
    public record GetSimcardByIdQuery(long Id) : IRequest<BusinessResult<SimcardUiModel>>;

    public class GetSimcardsQuery : GetSimcardsResourceParameters, IRequest<BusinessResult<PagedList<SimcardUiModel>>>
    {
        public GetSimcardsQuery(GetSimcardsResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            SortDirection = parameters.SortDirection;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    public class GetSimcardsByCompanyQuery : GetSimcardsResourceParameters, IRequest<BusinessResult<PagedList<SimcardUiModel>>> {
        public GetSimcardsByCompanyQuery(long companyId, GetSimcardsResourceParameters parameters) : base() {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            SortDirection = parameters.SortDirection;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateSimcardCommand(long CreatedById, CreateSimcardResourceParameters Parameters)
        : IRequest<BusinessResult<SimcardCreationUiModel>>;

    public record UpdateSimcardCommand(long Id, long ModifiedById, UpdateSimcardResourceParameters Parameters)
        : IRequest<BusinessResult<SimcardModificationUiModel>>;

    public record DeleteSoftSimcardCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<SimcardDeletionUiModel>>;

    public record DeleteHardSimcardCommand(long Id)
        : IRequest<BusinessResult<SimcardDeletionUiModel>>;
}