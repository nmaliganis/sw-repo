using sw.localization.contracts.ContractRepositories;
using sw.localization.model.Localizations;
using sw.localization.repository.Repositories.Base;
using System.Linq;

namespace sw.localization.repository.Repositories
{
    public class LocalizationDomainRepository : RepositoryBase<LocalizationDomain, long>, ILocalizationDomainRepository
    {
        public LocalizationDomainRepository(swDbContext context)
            : base(context)
        {
        }

        public bool HasDomain(string domain)
        {
            var result = Context.LocalizationDomains
                .Where(x => x.Name == domain
                         && x.Active)
                .Select(x => x)
                .Any();

            return result;
        }

        public void Save(LocalizationDomain entity)
        {
            throw new System.NotImplementedException();
        }
    }
}