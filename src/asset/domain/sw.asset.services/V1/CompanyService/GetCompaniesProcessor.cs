using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.ContractRepositories;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.asset.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sw.asset.model.Companies;

namespace sw.asset.services.V1.CompanyService
{
    public class GetCompaniesProcessor :
        IGetCompaniesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ICompanyRepository _companyRepository;

        public GetCompaniesProcessor(ICompanyRepository companyRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _companyRepository = companyRepository;
            _propertyMappingService = propertyMappingService;
            _autoMapper = autoMapper;
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
