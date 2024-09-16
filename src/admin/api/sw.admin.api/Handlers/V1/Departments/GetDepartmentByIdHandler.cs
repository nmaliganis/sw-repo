using sw.admin.common.dtos.V1.Cqrs.Departments;
using sw.admin.common.dtos.V1.Vms.Departments;
using sw.admin.contracts.V1.DepartmentProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using sw.landmark.common.dtos.V1.Vms.Departments;

namespace sw.admin.api.Handlers.V1.Departments
{
    internal class GetDepartmentByIdHandler :
        IRequestHandler<GetDepartmentByIdQuery, BusinessResult<DepartmentUiModel>>
    {
        private readonly IGetDepartmentByIdProcessor _processor;

        public GetDepartmentByIdHandler(IGetDepartmentByIdProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<DepartmentUiModel>> Handle(GetDepartmentByIdQuery qry, CancellationToken cancellationToken)
        {
            return await _processor.GetDepartmentByIdAsync(qry.Id);
        }
    }
}
