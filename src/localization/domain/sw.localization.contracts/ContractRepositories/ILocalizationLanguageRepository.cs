using sw.localization.model.Localizations;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.localization.contracts.ContractRepositories
{
    public interface ILocalizationLanguageRepository : IRepository<LocalizationLanguage, long>
    {
        bool HasLanguage(string language);
    }
}