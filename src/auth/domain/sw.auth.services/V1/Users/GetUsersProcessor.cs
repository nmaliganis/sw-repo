using sw.auth.common.dtos.Cqrs.Users;
using sw.auth.common.dtos.Vms.Users;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Users;
using sw.auth.model.Users;
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
using sw.infrastructure.Domain.Queries;

namespace sw.auth.services.V1.Users;

public class GetUsersProcessor : IGetUsersProcessor, 
    IRequestHandler<GetUsersQuery, BusinessResult<PagedList<UserUiModel>>>, 
    IRequestHandler<GetUsersByCompanyQuery, BusinessResult<PagedList<UserUiModel>>>,
    IRequestHandler<GetUsersByDepartmentQuery, BusinessResult<PagedList<UserUiModel>>>
{
    private readonly IAutoMapper _autoMapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IUserRepository _userRepository;

    public GetUsersProcessor(IUserRepository userRepository,
        IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
    {
        this._autoMapper = autoMapper;
        this._userRepository = userRepository;
        this._propertyMappingService = propertyMappingService;
    }

    public async Task<BusinessResult<PagedList<UserUiModel>>> Handle(GetUsersQuery qry, CancellationToken cancellationToken)
    {
        return await this.GetUsersAsync(qry);
    }

    public async Task<BusinessResult<PagedList<UserUiModel>>> GetUsersAsync(GetUsersQuery qry)
    {
        var collectionBeforePaging =
            this._userRepository.FindAllActiveUsersPagedOf(qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                    this._propertyMappingService.GetPropertyMapping<UserUiModel, User>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<User>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
        }

        return await BuildBusinessResult(qry.PageIndex, qry.PageSize, collectionBeforePaging);
    }

    private async Task<BusinessResult<PagedList<UserUiModel>>> BuildBusinessResult(int? pageIndex, int? pageSize, QueryResult<User> collectionBeforePaging)
    {
        var afterPaging = PagedList<User>
            .Create(collectionBeforePaging, pageIndex, pageSize);

        var items = this._autoMapper.Map<List<UserUiModel>>(afterPaging);

        var result = new PagedList<UserUiModel>(
            items,
            afterPaging.TotalCount,
            afterPaging.CurrentPage,
            afterPaging.PageSize);

        var bc = new BusinessResult<PagedList<UserUiModel>>(result);

        return await Task.FromResult(bc);
    }

    public async Task<BusinessResult<PagedList<UserUiModel>>> GetUsersByCompanyAsync(long companyId, GetUsersByCompanyQuery qry)
    {
        var collectionBeforePaging =
            this._userRepository.FindAllActiveUsersByCompanyPagedOf(companyId, qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                    this._propertyMappingService.GetPropertyMapping<UserUiModel, User>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<User>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
        }

        return await BuildBusinessResult(qry.PageIndex, qry.PageSize, collectionBeforePaging);
    }
    public async Task<BusinessResult<PagedList<UserUiModel>>> Handle(GetUsersByCompanyQuery qry, CancellationToken cancellationToken)
    {
        return await this.GetUsersByCompanyAsync(qry.CompanyId, qry);
    }

    public async Task<BusinessResult<PagedList<UserUiModel>>> GetUsersByDepartmentAsync(long departmentId, GetUsersByDepartmentQuery qry)
    {
        var collectionBeforePaging =
            this._userRepository.FindAllActiveUsersByDepartmentPagedOf(departmentId, qry.PageIndex, qry.PageSize)
                .ApplySort(qry.OrderBy + " " + qry.SortDirection,
                    this._propertyMappingService.GetPropertyMapping<UserUiModel, User>());

        if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
        {
            var searchQueryForWhereClauseFilterFields = qry.Filter
                .Trim().ToLowerInvariant();

            var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                .Trim().ToLowerInvariant();

            collectionBeforePaging.QueriedItems = (IQueryable<User>)collectionBeforePaging
                .QueriedItems
                .AsEnumerable()
                .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
        }

        return await BuildBusinessResult(qry.PageIndex, qry.PageSize, collectionBeforePaging);
    }


    public async Task<BusinessResult<PagedList<UserUiModel>>> Handle(GetUsersByDepartmentQuery qry, CancellationToken cancellationToken)
    {
        return await this.GetUsersByDepartmentAsync(qry.DepartmentId, qry);
    }
}