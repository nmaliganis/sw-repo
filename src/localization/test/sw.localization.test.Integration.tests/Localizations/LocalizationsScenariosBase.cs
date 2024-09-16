using System;
using dottrack.localization.test.Integration.tests.Base;

namespace dottrack.test.Integration.tests.Localizations
{
    public class LocalizationsScenariosBase : RegisterUserScenariosBase
    {
        private const string LocalizationApiUrlBase = "/api/v1/LocalizationValues";

        public static class Get
        {
            public static string GetLocalizationValueById(long id)
            {
                return $"{LocalizationApiUrlBase}/{id}";
            }

            public static string GetLocalizationValueByKeyAndDomainAndLang(string key, string domain, string lang)
            {
                return $"{LocalizationApiUrlBase}/{key}/domain/{domain}/language/{lang}";
            }

            public static string GetLocalizationValues(string domain, string lang)
            {
                return $"{LocalizationApiUrlBase}/domain/{domain}/language/{lang}";
            }
        }
        public static class Post
        {
            public static string PostLocalization()
            {
                return $"{LocalizationApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string PutLocalization(long id)
            {
                return $"{LocalizationApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftLocalization(long id)
            {
                return $"{LocalizationApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardLocalization(long id)
            {
                return $"{LocalizationApiUrlBase}/hard/{id}";
            }
        }
    }
}
