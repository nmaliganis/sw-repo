using sw.localization.contracts.ContractRepositories;
using sw.localization.model.Localizations;
using sw.localization.repository.Repositories.Base;
using System;
using System.Linq;

namespace sw.localization.repository.Repositories
{
    public class LocalizationLanguageRepository : RepositoryBase<LocalizationLanguage, long>, ILocalizationLanguageRepository
    {
        public LocalizationLanguageRepository(swDbContext context)
            : base(context)
        {
        }

        public bool HasLanguage(string language)
        {
            var result = Context.LocalizationLanguages
                .Where(x => x.Name == language
                         && x.Active)
                .Select(x => x)
                .Any();

            return result;
        }

        public void Save(LocalizationLanguage entity)
        {
            throw new NotImplementedException();
        }
    }
}