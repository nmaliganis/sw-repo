using sw.localization.model.Localizations;
using sw.infrastructure.Domain;

namespace sw.localization.contracts.ContractRepositories
{
    public interface ILocalizationDomainRepository : IRepository<LocalizationDomain, long>
    {
        bool HasDomain(string domain);
    }
}