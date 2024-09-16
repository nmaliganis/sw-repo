using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.admin.services.V1.CompanyService
{
    public class GetCompaniesProcessor : IGetCompaniesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ICompanyRepository _companyRepository;

        public GetCompaniesProcessor(ICompanyRepository companyRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _companyRepository = companyRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<CompanyUiModel>>> GetCompaniesAsync(GetCompaniesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_companyRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<CompanyUiModel, Company>());

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

            var afterPaging = PagedList<Company>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<CompanyUiModel>>(afterPaging);

            var result = new PagedList<CompanyUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<CompanyUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
