using sw.admin.common.dtos.V1.Cqrs.DepartmentPersonRoles;
using sw.admin.common.dtos.V1.Vms.DepartmentPersonRoles;
using sw.admin.contracts.ContractRepositories;
using sw.admin.contracts.V1.DepartmentPersonRoleProcessors;
using sw.admin.model;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sw.admin.services.V1.DepartmentPersonRoleService
{
    public class GetDepartmentPersonRolesProcessor : IGetDepartmentPersonRolesProcessor
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IDepartmentPersonRoleRepository _departmentPersonRoleRepository;

        public GetDepartmentPersonRolesProcessor(IDepartmentPersonRoleRepository departmentPersonRoleRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            _departmentPersonRoleRepository = departmentPersonRoleRepository;
            _autoMapper = autoMapper;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<DepartmentPersonRoleUiModel>>> GetDepartmentPersonRolesAsync(GetDepartmentPersonRolesQuery qry)
        {
            var collectionBeforePaging =
                QueryableExtensions.ApplySort(_departmentPersonRoleRepository.FindAllActivePagedOf(qry.PageIndex,qry.PageSize),
                    qry.OrderBy + " " + qry.SortDirection,
                    _propertyMappingService.GetPropertyMapping<DepartmentPersonRoleUiModel, DepartmentPersonRole>());

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

            var afterPaging = PagedList<DepartmentPersonRole>
                .Create(collectionBeforePaging, qry.PageIndex, qry.PageSize);

            var items = _autoMapper.Map<List<DepartmentPersonRoleUiModel>>(afterPaging);

            var result = new PagedList<DepartmentPersonRoleUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<DepartmentPersonRoleUiModel>>(result);

            return await Task.FromResult(bc);
        }
    }
}
