using dottrack.auth.Integration.tests.Base;

namespace dottrack.auth.Integration.tests.V1.Role
{
    public class RolesScenariosBase : RegisterUserScenariosBase
    {
        private const string RoleApiUrlBase = "/api/v1/Roles";

        public static class Get
        {
            public static string GetRoleByIdAsync(long id)
            {
                return $"{RoleApiUrlBase}/{id}";
            }

            public static string GetRolesAsync()
            {
                return $"{RoleApiUrlBase}";
            }
        }

        public static class Post
        {
            public static string PostRoleAsync()
            {
                return $"{RoleApiUrlBase}";
            }
        }

        public static class Put
        {
            public static string UpdateRoleAsync(long id)
            {
                return $"{RoleApiUrlBase}/{id}";
            }
        }
        public static class Delete
        {
            public static string DeleteSoftRoleAsync(long id)
            {
                return $"{RoleApiUrlBase}/soft/{id}";
            }
            public static string DeleteHardRoleAsync(long id)
            {
                return $"{RoleApiUrlBase}/hard/{id}";
            }
        }
    }
}
