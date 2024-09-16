using sw.localization.common.dtos.Cqrs.LocalizationValues;
using sw.localization.common.dtos.Vms.LocalizationValues;
using sw.localization.contracts.ContractRepositories;
using sw.localization.contracts.V1.LocalizationValueProcessors;
using sw.localization.model.Localizations;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.localization.services.LocalizationValueService
{
    public class GetLocalizationValuesProcessor :
        IGetLocalizationValuesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILocalizationValueRepository _localizationValueRepository;

        public GetLocalizationValuesProcessor(ILocalizationValueRepository localizationValueRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _localizationValueRepository = localizationValueRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public Task<PagedList<LocalizationValueUiModel>> GetLocalizationValuesAsync(GetLocalizationValuesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_localizationValueRepository.FindAllLocalesPagedOf(qry.PageIndex, qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<LocalizationValueUiModel, LocalizationValue>());

            if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = qry.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<LocalizationValue>) collectionBeforePaging
                    .QueriedItems
                    .AsEnumerable()
                    .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
            }

            var afterPaging = PagedList<LocalizationValue>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<LocalizationValueUiModel>>(afterPaging);

            var result = new PagedList<LocalizationValueUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            return Task.FromResult(result);
        }
    }
}