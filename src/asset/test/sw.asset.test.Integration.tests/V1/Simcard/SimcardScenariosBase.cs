using dottrack.asset.test.Integration.tests.Base;

namespace dottrack.asset.Integration.tests.V1.Simcard
{
    public class SimcardScenariosBase : RegisterUserScenariosBase
    {
        private const string SimcardApiUrlBase = "/api/v1/Simcards";

        public static class Get
        {
            public static string GetSimcardByIdAsync(long id)
            {
                return $"{SimcardApiUrlBase}/{id}";
            }

            public static string GetSimcardsAsync()
            {
                return $"{SimcardApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostSimcardAsync()
            {
                return $"{SimcardApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateSimcardAsync(long id)
            {
                return $"{SimcardApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftSimcardAsync(long id)
            {
                return $"{SimcardApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardSimcardAsync(long id)
            {
                return $"{SimcardApiUrlBase}/hard/{id}";
            }
        }
    }
}
