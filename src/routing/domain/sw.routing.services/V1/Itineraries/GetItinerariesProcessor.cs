using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.Itineraries;
using sw.routing.common.dtos.Vms.Itineraries;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Itineraries;
using sw.routing.model.Itineraries;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using MediatR;

namespace sw.routing.services.V1.Itineraries;

public class GetItinerariesProcessor : IGetItinerariesProcessor, IRequestHandler<GetItinerariesQuery, BusinessResult<PagedList<ItineraryUiModel>>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IItineraryRepository _itineraryRepository;

    public GetItinerariesProcessor(IItineraryRepository itineraryRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _itineraryRepository = itineraryRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<ItineraryUiModel>>> Handle(GetItinerariesQuery qry, CancellationToken cancellationToken)
    {
        return await GetItinerariesAsync(qry);
    }

    public async Task<BusinessResult<PagedList<ItineraryUiModel>>> GetItinerariesAsync(GetItinerariesQuery qry)
    {
        var collectionBeforePaging =
            _itineraryRepository.FindAllActiveItinerariesPagedOf(qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                _propertyMappingService.GetPropertyMapping<ItineraryUiModel, Itinerary>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<Itinerary>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
        }

        var afterPaging = PagedList<Itinerary>
            .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<ItineraryUiModel>>(afterPaging);

        var result = new PagedList<ItineraryUiModel>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<ItineraryUiModel>>(result);

        return await Task.FromResult(bc);
    }
}