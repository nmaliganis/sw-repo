using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Companies;

internal class UpdateCompanyHandler :
    IRequestHandler<UpdateCompanyCommand, BusinessResult<CompanyModificationUiModel>>,
    IRequestHandler<UpdateCompanyWithZoneCommand, BusinessResult<CompanyModificationUiModel>>
{
    private readonly IUpdateCompanyProcessor _processor;

    public UpdateCompanyHandler(IUpdateCompanyProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<CompanyModificationUiModel>> Handle(UpdateCompanyCommand updateCommand, CancellationToken cancellationToken)
    {
        return await _processor.UpdateCompanyAsync(updateCommand);
    }

    public async Task<BusinessResult<CompanyModificationUiModel>> Handle(UpdateCompanyWithZoneCommand request, CancellationToken cancellationToken)
    {
        return await _processor.UpdateCompanyWithZoneAsync(request);
    }
}
