using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Companies;

internal class CreateCompanyHandler :
    IRequestHandler<CreateCompanyCommand, BusinessResult<CompanyCreationUiModel>>
{
    private readonly ICreateCompanyProcessor _processor;

    public CreateCompanyHandler(ICreateCompanyProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<CompanyCreationUiModel>> Handle(CreateCompanyCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateCompanyAsync(createCommand);
    }

}
