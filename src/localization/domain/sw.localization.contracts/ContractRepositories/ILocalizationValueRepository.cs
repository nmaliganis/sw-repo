using sw.localization.model.Localizations;
using sw.infrastructure.Domain;
using sw.infrastructure.Domain.Queries;

namespace sw.localization.contracts.ContractRepositories
{
    public interface ILocalizationValueRepository : IRepository<LocalizationValue, long>
    {
        QueryResult<LocalizationValue> FindAllLocalesPagedOf(int? pageNum, int? pageSize);
        LocalizationValue GetLocaleByKey(string key, string domain, string language);
        (bool exists, LocalizationValue localizationValue) HasKey(long domainId, long languageId, string key);
        long? FindLanguageId(string language);
        long? FindDomainId(string domain);
    }
}