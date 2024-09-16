using sw.auth.common.dtos.Cqrs.Companies;
using sw.auth.common.dtos.Vms.Companies;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Companies;
using sw.auth.model.Companies;
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

namespace sw.auth.services.V1.Companies;

public class GetCompaniesByUserProcessor : IGetCompaniesByUserProcessor,
    IRequestHandler<GetCompaniesByUserQuery, BusinessResult<PagedList<CompanyUiModel>>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly ICompanyRepository _companyRepository;

    public GetCompaniesByUserProcessor(ICompanyRepository companyRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        _companyRepository = companyRepository;
        _autoMapper = autoMapper;
        _propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<CompanyUiModel>>> Handle(GetCompaniesByUserQuery qry, CancellationToken cancellationToken)
    {
        return await GetCompaniesByUserAsync(qry);
    }

    public async Task<BusinessResult<PagedList<CompanyUiModel>>> GetCompaniesByUserAsync(GetCompaniesByUserQuery qry)
    {
        var collectionBeforePaging =
            _companyRepository.FindAllActiveCompaniesByUserPagedOf(qry.CompanyId, qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<CompanyUiModel, Company>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<Company>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
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