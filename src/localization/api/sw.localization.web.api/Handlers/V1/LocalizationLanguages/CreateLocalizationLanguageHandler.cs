using sw.localization.common.dtos.Cqrs.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.contracts.V1.LocalizationLanguageProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationLanguages;

internal class CreateLocalizationLanguageHandler :
    IRequestHandler<CreateLocalizationLanguageCommand, BusinessResult<LocalizationLanguageCreationUiModel>>
{
    private readonly ICreateLocalizationLanguageProcessor _processor;

    public CreateLocalizationLanguageHandler(ICreateLocalizationLanguageProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<LocalizationLanguageCreationUiModel>> Handle(CreateLocalizationLanguageCommand createCommand, CancellationToken cancellationToken)
    {
        return await _processor.CreateLocalizationLanguageAsync(createCommand);
    }
}