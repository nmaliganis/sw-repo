using sw.localization.common.dtos.Cqrs.LocalizationDomains;
using sw.localization.common.dtos.Vms.LocalizationDomains;
using sw.infrastructure.BrokenRules;
using System.Threading.Tasks;

namespace sw.localization.contracts.V1.LocalizationDomainProcessors
{
    public interface ICreateLocalizationDomainProcessor
    {
        Task<BusinessResult<LocalizationDomainCreationUiModel>> CreateLocalizationDomainAsync(CreateLocalizationDomainCommand createCommand);
    }
}