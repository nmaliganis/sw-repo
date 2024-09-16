using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.routing.common.dtos.Cqrs.ItineraryTemplates;
using sw.routing.common.dtos.Vms.ItineraryTemplates;
using sw.routing.contracts.ContractRepositories;
using sw.routing.contracts.V1.Itineraries;
using sw.routing.contracts.V1.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using MediatR;

namespace sw.routing.services.V1.ItineraryTemplates;

public class GetItineraryTemplatesProcessor : IGetItineraryTemplatesProcessor, IRequestHandler<GetItineraryTemplatesQuery, BusinessResult<PagedList<ItineraryTemplateUiModel>>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IItineraryTemplateRepository _itineraryTemplateRepository;

    public GetItineraryTemplatesProcessor(IItineraryTemplateRepository itineraryTemplateRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _itineraryTemplateRepository = itineraryTemplateRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<ItineraryTemplateUiModel>>> Handle(GetItineraryTemplatesQuery qry, CancellationToken cancellationToken)
    {
        return await GetItineraryTemplatesAsync(qry);
    }

    public async Task<BusinessResult<PagedList<ItineraryTemplateUiModel>>> GetItineraryTemplatesAsync(GetItineraryTemplatesQuery qry)
    {
        var collectionBeforePaging =
            _itineraryTemplateRepository.FindAllActiveItineraryTemplatesPagedOf(qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                _propertyMappingService.GetPropertyMapping<ItineraryTemplateUiModel, ItineraryTemplate>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<ItineraryTemplate>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
        }

        var afterPaging = PagedList<ItineraryTemplate>
            .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

        var items = _autoMapper.Map<List<ItineraryTemplateUiModel>>(afterPaging);

        var result = new PagedList<ItineraryTemplateUiModel>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<ItineraryTemplateUiModel>>(result);

        return await Task.FromResult(bc);
    }
}