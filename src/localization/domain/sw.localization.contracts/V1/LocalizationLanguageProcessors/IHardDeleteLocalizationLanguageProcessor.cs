using sw.localization.common.dtos.Cqrs.LocalizationLanguages;
using sw.localization.common.dtos.Vms.LocalizationLanguages;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.localization.contracts.V1.LocalizationLanguageProcessors
{
    public interface IHardDeleteLocalizationLanguageProcessor
    {
        Task<BusinessResult<LocalizationLanguageDeletionUiModel>> HardDeleteLocalizationLanguageAsync(HardDeleteLocalizationLanguageCommand deleteCommand);
    }
}