using sw.localization.common.dtos.Cqrs.LocalizationDomains;
using sw.localization.common.dtos.Vms.LocalizationDomains;
using sw.localization.contracts.V1.LocalizationDomainProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationDomains;

internal class CreateLocalizationDomainHandler :
    IRequestHandler<CreateLocalizationDomainCommand, BusinessResult<LocalizationDomainCreationUiModel>>
{
    private readonly ICreateLocalizationDomainProcessor _processor;

    public CreateLocalizationDomainHandler(ICreateLocalizationDomainProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<LocalizationDomainCreationUiModel>> Handle(CreateLocalizationDomainCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateLocalizationDomainAsync(createCommand);
    }
}