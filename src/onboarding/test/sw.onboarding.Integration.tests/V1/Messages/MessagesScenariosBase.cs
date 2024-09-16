using dottrack.auth.Integration.tests.Base;

namespace dottrack.auth.Integration.tests.V1.Message
{
    public class MessagesScenariosBase : RegisterUserScenariosBase
    {
        private const string MessageApiUrlBase = "/api/Messages/propagate";


        public static class Post
        {
            public static string PostMessageAsync()
            {
                return $"{MessageApiUrlBase}";
            }
        }

    }
}
