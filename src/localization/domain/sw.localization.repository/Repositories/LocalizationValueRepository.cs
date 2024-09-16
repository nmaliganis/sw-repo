using sw.localization.contracts.ContractRepositories;
using sw.localization.model.Localizations;
using sw.localization.repository.Repositories.Base;
using sw.infrastructure.Domain.Queries;
using sw.infrastructure.Paging;
using System;
using System.Linq;

namespace sw.localization.repository.Repositories
{
    public class LocalizationValueRepository : RepositoryBase<LocalizationValue, long>, ILocalizationValueRepository
    {
        public LocalizationValueRepository(swDbContext context)
            : base(context)
        {
        }

        public LocalizationValue GetLocaleByKey(string key, string domain, string language)
        {
            return (from lva in Context.LocalizationValues
                    join lla in Context.LocalizationLanguages
                        on new { langId = lva.LanguageId, active = true } equals new { langId = lla.Id, active = lla.Active }
                    join ldo in Context.LocalizationDomains
                        on new { domainId = lva.DomainId, active = true }  equals new { domainId = ldo.Id, active = ldo.Active }

                    where lla.Name == language
                       && ldo.Name == domain
                       && lva.Key == key
                       && lva.Active

                    orderby lva.Key
                    select lva
                    ).FirstOrDefault();
        }

        public QueryResult<LocalizationValue> FindAllLocalesPagedOf(int? pageNum, int? pageSize)
        {
            var qry = (from lva in Context.LocalizationValues
                       join lla in Context.LocalizationLanguages on lva.LanguageId equals lla.Id
                       join ldo in Context.LocalizationDomains on lva.DomainId equals ldo.Id
                       where lva.Active
                       select lva);

            if (pageNum == -1 & pageSize == -1)
            {
                return new QueryResult<LocalizationValue>(qry.AsQueryable());
            }

            var result = qry.Skip(ResultsPagingUtility.CalculateStartIndex((int)pageNum, (int)pageSize))
                            .Take((int)pageSize)
                            .AsQueryable();

            return new QueryResult<LocalizationValue>(result, qry.Count(), (int)pageSize);
        }

        public (bool exists, LocalizationValue localizationValue) HasKey(long domainId, long languageId, string key)
        {
            var qry = Context.LocalizationValues
                .Where(x => x.DomainId == domainId
                         && x.LanguageId == languageId
                         && x.Key == key
                         && x.Active)
                .Select(x => x)
                .FirstOrDefault();

            return (qry is not null, qry);
        }

        public long? FindLanguageId(string language)
        {
            long? languageId = Context.LocalizationLanguages
                .Where(x => x.Name == language
                         && x.Active)
                .Select(x => x.Id)
                .FirstOrDefault();

            return languageId;
        }

        public long? FindDomainId(string domain)
        {
            long? domainId = Context.LocalizationDomains
                .Where(x => x.Name == domain
                         && x.Active)
                .Select(x => x.Id)
                .FirstOrDefault();

            return domainId;
        }

        public void Save(LocalizationValue entity)
        {
            throw new NotImplementedException();
        }
    }
}