using AutoMapper;
using dottrack.asset.Integration.tests.Base;

namespace dottrack.asset.test.Integration.tests.Base
{
  public class RegisterUserScenariosBase : BaseServer
  {
    protected const string JwtApiUrlBase = "api/UserJwt/authtoken";

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
