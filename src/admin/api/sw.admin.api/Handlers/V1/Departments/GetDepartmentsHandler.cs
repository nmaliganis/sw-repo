using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using sw.landmark.common.dtos.V1.Vms.Departments;

namespace sw.admin.api.Handlers.V1.Departments
{
    internal class GetDepartmentsHandler :
        IRequestHandler<GetDepartmentsQuery, BusinessResult<PagedList<DepartmentUiModel>>>
    {
        private readonly IGetDepartmentsProcessor _processor;

        public GetDepartmentsHandler(IGetDepartmentsProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PagedList<DepartmentUiModel>>> Handle(GetDepartmentsQuery qry, CancellationToken cancellationToken)
        {
            return await _processor.GetDepartmentsAsync(qry);
        }
    }
}
