using AutoMapper;
using dottrack.test.Integration.tests.Base;

namespace dottrack.localization.test.Integration.tests.Base
{
  public class RegisterUserScenariosBase : BaseServer
  {
    protected const string JwtApiUrlBase = "/api/users/login";

    public static class PostJwt
    {
      public static string PostLogin()
      {
        return $"{JwtApiUrlBase}";
      }
    }

    protected void Startup()
    {
      Mapper.Reset();
    }

  }
}
