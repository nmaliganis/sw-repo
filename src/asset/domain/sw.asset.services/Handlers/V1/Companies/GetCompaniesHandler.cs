using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using sw.infrastructure.Paging;
using MediatR;

namespace sw.asset.services.Handlers.V1.Companies;

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
