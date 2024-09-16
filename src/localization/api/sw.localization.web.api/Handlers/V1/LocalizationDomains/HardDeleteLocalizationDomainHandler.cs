using sw.localization.common.dtos.Cqrs.LocalizationDomains;
using sw.localization.common.dtos.Vms.LocalizationDomains;
using sw.localization.contracts.V1.LocalizationDomainProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationDomains;

internal class HardDeleteLocalizationDomainHandler :
    IRequestHandler<HardDeleteLocalizationDomainCommand, BusinessResult<LocalizationDomainDeletionUiModel>>
{
    private readonly IHardDeleteLocalizationDomainProcessor _processor;

    public HardDeleteLocalizationDomainHandler(IHardDeleteLocalizationDomainProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<LocalizationDomainDeletionUiModel>> Handle(HardDeleteLocalizationDomainCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.HardDeleteLocalizationDomainAsync(deleteCommand);
    }
}