using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using sw.auth.common.dtos.Vms.Roles;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Roles;
using sw.auth.model.Roles;
using sw.onboarding.common.dtos.Cqrs.Roles;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using MediatR;

namespace sw.onboarding.services.V1.Roles
{
    public class GetRolesProcessor : IGetRolesProcessor,
        IRequestHandler<GetRolesQuery, BusinessResult<PagedList<RoleUiModel>>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IRoleRepository _roleRepository;

        public GetRolesProcessor(IRoleRepository roleRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _roleRepository = roleRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<RoleUiModel>>> Handle(GetRolesQuery qry, CancellationToken cancellationToken)
        {
            return await GetRolesAsync(qry);
        }

        public async Task<BusinessResult<PagedList<RoleUiModel>>> GetRolesAsync(GetRolesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_roleRepository.FindAllActiveRolesPagedOf(qry.PageIndex, qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<RoleUiModel, Role>());

            if (!string.IsNullOrEmpty(qry.Filter) && !string.IsNullOrEmpty(qry.SearchQuery))
            {
                var searchQueryForWhereClauseFilterFields = qry.Filter
                    .Trim().ToLowerInvariant();

                var searchQueryForWhereClauseFilterSearchQuery = qry.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging.QueriedItems = (IQueryable<Role>)collectionBeforePaging
                    .QueriedItems
                    .AsEnumerable()
                    .FilterData(searchQueryForWhereClauseFilterFields, searchQueryForWhereClauseFilterSearchQuery);
            }

            var afterPaging = PagedList<Role>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<RoleUiModel>>(afterPaging);

            var result = new PagedList<RoleUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<RoleUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
