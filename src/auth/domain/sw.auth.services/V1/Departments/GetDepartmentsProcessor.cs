using sw.auth.common.dtos.Cqrs.Departments;
using sw.auth.common.dtos.Vms.Departments;
using sw.auth.contracts.ContractRepositories;
using sw.auth.contracts.V1.Departments;
using sw.auth.model.Departments;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Extensions;
using sw.infrastructure.Paging;
using sw.infrastructure.PropertyMappings;
using sw.infrastructure.TypeMappings;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace sw.auth.services.V1.Departments
{
    public class GetDepartmentsProcessor : IGetDepartmentsProcessor,
        IRequestHandler<GetDepartmentsQuery, BusinessResult<PagedList<DepartmentUiModel>>>,
        IRequestHandler<GetDepartmentsByCompanyQuery, BusinessResult<PagedList<DepartmentUiModel>>>
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IDepartmentRepository _departmentRepository;

        public GetDepartmentsProcessor(IDepartmentRepository departmentRepository,
            IAutoMapper autoMapper, IPropertyMappingService propertyMappingService)
        {
            this._departmentRepository = departmentRepository;
            this._autoMapper = autoMapper;
            this._propertyMappingService = propertyMappingService;
        }

        public async Task<BusinessResult<PagedList<DepartmentUiModel>>> Handle(GetDepartmentsQuery qry, CancellationToken cancellationToken)
        {
            return await this.GetDepartmentsAsync(qry);
        }

        public async Task<BusinessResult<PagedList<DepartmentUiModel>>> GetDepartmentsAsync(GetDepartmentsQuery qry)
        {
            var collectionBeforePaging =
                this._departmentRepository.FindAllActivePagedOf(qry.PageIndex, qry.PageSize).ApplySort(qry.OrderBy + " " + qry.SortDirection,
                    this._propertyMappingService.GetPropertyMapping<DepartmentUiModel, Department>());

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

            return await CreationBusinessResult(qry.PageIndex, qry.PageSize, collectionBeforePaging);
        }

        public async Task<BusinessResult<PagedList<DepartmentUiModel>>> GetDepartmentsByCompanyAsync(GetDepartmentsByCompanyQuery qry)
        {
            var collectionBeforePaging =
                this._departmentRepository.FindAllActiveByCompanyPagedOf(qry.CompanyId, qry.PageIndex, qry.PageSize).ApplySort(qry.OrderBy + " " + qry.SortDirection,
                    this._propertyMappingService.GetPropertyMapping<DepartmentUiModel, Department>());

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

            return await CreationBusinessResult(qry.PageIndex, qry.PageSize, collectionBeforePaging);
        }

        private async Task<BusinessResult<PagedList<DepartmentUiModel>>>
            CreationBusinessResult(int? pageIndex, int? pageSize, QueryResult<Department> collectionBeforePaging)
        {
            var afterPaging = PagedList<Department>
                .Create(collectionBeforePaging, pageIndex, pageSize);

            var items = this._autoMapper.Map<List<DepartmentUiModel>>(afterPaging);

            var result = new PagedList<DepartmentUiModel>(
                items,
                afterPaging.TotalCount,
                afterPaging.CurrentPage,
                afterPaging.PageSize);

            var bc = new BusinessResult<PagedList<DepartmentUiModel>>(result);

            return await Task.FromResult(bc);
        }


        public async Task<BusinessResult<PagedList<DepartmentUiModel>>> Handle(GetDepartmentsByCompanyQuery qry, CancellationToken cancellationToken)
        {
            return await this.GetDepartmentsByCompanyAsync(qry);
        }
    }
}
