using System.Threading;
using System.Threading.Tasks;
using sw.admin.common.dtos.V1.Cqrs.Companies;
using sw.admin.common.dtos.V1.Vms.Companies;
using sw.admin.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.admin.api.Handlers.V1.Companies
{
    internal class GetCompaniesHandler :
        IRequestHandler<GetCompaniesQuery, BusinessResult<PagedList<CompanyUiModel>>>
    {
        private readonly IGetCompaniesProcessor _processor;

        public GetCompaniesHandler(IGetCompaniesProcessor processor)
        {
            _processor = processor;
        }

        public async Task<BusinessResult<PagedList<CompanyUiModel>>> Handle(GetCompaniesQuery qry, CancellationToken cancellationToken)
        {
            return await _processor.GetCompaniesAsync(qry);
        }
    }
}
