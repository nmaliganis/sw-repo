using sw.localization.common.dtos.Cqrs.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.localization.contracts.V1.LocalizationLanguageProcessors;
using sw.infrastructure.BrokenRules;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace sw.localization.api.Handlers.V1.LocalizationLanguages;

internal class HardDeleteLocalizationLanguageHandler :
    IRequestHandler<HardDeleteLocalizationLanguageCommand, BusinessResult<LocalizationLanguageDeletionUiModel>>
{
    private readonly IHardDeleteLocalizationLanguageProcessor _processor;

    public HardDeleteLocalizationLanguageHandler(IHardDeleteLocalizationLanguageProcessor processor)
    {
        _processor = processor;
    }

    public async Task<BusinessResult<LocalizationLanguageDeletionUiModel>> Handle(HardDeleteLocalizationLanguageCommand deleteCommand, CancellationToken cancellationToken)
    {
        return await _processor.HardDeleteLocalizationLanguageAsync(deleteCommand);
    }
}