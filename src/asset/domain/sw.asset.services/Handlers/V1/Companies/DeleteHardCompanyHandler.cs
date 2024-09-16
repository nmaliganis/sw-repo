using System.Threading;
using System.Threading.Tasks;
using sw.asset.common.dtos.Cqrs.Companies;
using sw.asset.common.dtos.Vms.Companies;
using sw.asset.contracts.V1.CompanyProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;

namespace sw.asset.services.Handlers.V1.Companies;

internal class DeleteHardCompanyHandler :
    IRequestHandler<DeleteHardCompanyCommand, BusinessResult<CompanyDeletionUiModel>>
{
    private readonly IDeleteHardCompanyProcessor _processor;

    public DeleteHardCompanyHandler(IDeleteHardCompanyProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<CompanyDeletionUiModel>> Handle(DeleteHardCompanyCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.DeleteHardCompanyAsync(deleteCommand);
    }
}
