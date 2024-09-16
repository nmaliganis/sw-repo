using AutoMapper;

namespace dottrack.auth.Integration.tests.Base {
    public class RegisterUserScenariosBase : BaseServer {
        protected const string JwtApiUrlBase = "api/UserJwt/authtoken";

        public static class PostJwt {
            public static string PostLogin() {
                return $"{JwtApiUrlBase}";
            }
        }

        protected void Startup() {
            Mapper.Reset();
        }
    }
}
