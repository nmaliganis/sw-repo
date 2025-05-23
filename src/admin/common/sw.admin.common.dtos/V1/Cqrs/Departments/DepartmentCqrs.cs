﻿using sw.admin.common.dtos.V1.ResourceParameters.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.landmark.common.dtos.V1.Vms.Departments;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.admin.common.dtos.V1.Cqrs.Departments
{
    // Queries
    public record GetDepartmentByIdQuery(long Id) : IRequest<BusinessResult<DepartmentUiModel>>;

    public class GetDepartmentsQuery : GetDepartmentsResourceParameters, IRequest<BusinessResult<PagedList<DepartmentUiModel>>>
    {
        public GetDepartmentsQuery(GetDepartmentsResourceParameters parameters) : base()
        {
            Filter = parameters.Filter;
            SearchQuery = parameters.SearchQuery;
            Fields = parameters.Fields;
            OrderBy = parameters.OrderBy;
            SortDirection = parameters.SortDirection;
            PageSize = parameters.PageSize;
            PageIndex = parameters.PageIndex;
        }
    }

    // Commands
    public record CreateDepartmentCommand(long CreatedById, CreateDepartmentResourceParameters Parameters)
        : IRequest<BusinessResult<DepartmentCreationUiModel>>;

    public record UpdateDepartmentCommand(long Id, long ModifiedById, UpdateDepartmentResourceParameters Parameters)
        : IRequest<BusinessResult<DepartmentModificationUiModel>>;

    public record DeleteSoftDepartmentCommand(long Id, long DeletedBy, string DeletedReason)
        : IRequest<BusinessResult<DepartmentDeletionUiModel>>;

    public record DeleteHardDepartmentCommand(long Id)
        : IRequest<BusinessResult<DepartmentDeletionUiModel>>;
}
