using dottrack.auth.Integration.tests.Base;

namespace dottrack.auth.Integration.tests.V1.Account
{
    public class AccountsScenariosBase : RegisterUserScenariosBase
    {
        private const string AccountApiUrlBase = "/api/Accounts/register";


        public static class Post
        {
            public static string PostAccountRegisterAsync()
            {
                return $"{AccountApiUrlBase}";
            }
        }

    }
}
