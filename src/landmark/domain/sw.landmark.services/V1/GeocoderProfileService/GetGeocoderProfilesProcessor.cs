using sw.landmark.common.dtos.V1.Cqrs.GeocoderProfiles;
using sw.landmark.common.dtos.V1.Vms.GeocoderProfiles;
using sw.landmark.contracts.ContractRepositories;
using sw.landmark.contracts.V1.GeocoderProfileProcessors;
using sw.landmark.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sw.landmark.services.V1.GeocoderProfileService
{
    public class GetGeocoderProfilesProcessor :
        IGetGeocoderProfilesProcessor,
        IRequestHandler<GetGeocoderProfilesQuery, BusinessResult<PagedList<GeocoderProfileUiModel>>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IGeocoderProfileRepository _geocoderProfileRepository;

        public GetGeocoderProfilesProcessor(IGeocoderProfileRepository geocoderProfileRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _geocoderProfileRepository = geocoderProfileRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<GeocoderProfileUiModel>>> Handle(GetGeocoderProfilesQuery qry, CancellationToken cancellationToken)
        {
            return await GetGeocoderProfilesAsync(qry);
        }

        public async Task<BusinessResult<PagedList<GeocoderProfileUiModel>>> GetGeocoderProfilesAsync(GetGeocoderProfilesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_geocoderProfileRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<GeocoderProfileUiModel, GeocoderProfile>());

            if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = qry.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = collectionBeforePaging
                    .QueriedItems
                    .AsEnumerable()
                    .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery)
                    .AsQueryable();
            }

            var afterPaging = PagedList<GeocoderProfile>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<GeocoderProfileUiModel>>(afterPaging);

            var result = new PagedList<GeocoderProfileUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<GeocoderProfileUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
