using System.Threading;
using System.Threading.Tasks;
using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.admin.api.Handlers.V1.Companies
{
    internal class GetCompanyByIdHandler :
        IRequestHandler<GetCompanyByIdQuery, BusinessResult<CompanyUiModel>>
    {
        private readonly IGetCompanyByIdProcessor _processor;

        public GetCompanyByIdHandler(IGetCompanyByIdProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<CompanyUiModel>> Handle(GetCompanyByIdQuery qry, CancellationToken cancellationToken)
        {
            return await _processor.GetCompanyByIdAsync(qry.Id);
        }
    }
}
