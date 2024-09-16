using dottrack.asset.Integration.tests.Base.Auth;
using FluentAssertions;
using Newtonsoft.Json;
using NHibernate.Id.Insert;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static dottrack.asset.test.Integration.tests.Base.RegisterUserScenariosBase;

namespace dottrack.asset.Integration.tests.Base
{
    internal class GetAuthToken
    {


        public async static Task<string> GetToken(HttpClient client)
        {
            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);

            return authUiModel!.Token;
        }


    }
}
