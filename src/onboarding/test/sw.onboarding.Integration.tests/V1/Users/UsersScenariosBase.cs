using dottrack.auth.Integration.tests.Base;

namespace dottrack.auth.Integration.tests.V1.User
{
    public class UsersScenariosBase : RegisterUserScenariosBase
    {
        private const string UserApiUrlBase = "/api/v1/Users";

        public static class Get
        {
            public static string GetUserByIdAsync(long id)
            {
                return $"{UserApiUrlBase}/{id}";
            }

            public static string GetUsersAsync()
            {
                return $"{UserApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostUserAsync()
            {
                return $"{UserApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateUserAsync(long id)
            {
                return $"{UserApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftUserAsync(long id)
            {
                return $"{UserApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardUserAsync(long id)
            {
                return $"{UserApiUrlBase}/hard/{id}";
            }
        }
    }
}

