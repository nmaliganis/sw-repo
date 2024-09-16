using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Companies;

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
