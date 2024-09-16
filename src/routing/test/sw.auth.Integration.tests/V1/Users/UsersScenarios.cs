using AutoMapper;
using dottrack.auth.Integration.tests.Base;
using dottrack.auth.Integration.tests.V1.User;
using dottrack.common.dtos.ResourceParameters.Users;
using dottrack.common.dtos.Vms.Accounts;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using dottrack.auth.common.dtos.Vms.Users;
using Xunit;

namespace dottrack.auth.Integration.tests.V1.Users
{
    [Collection("Sequential")]
    public class UsersScenarios : UsersScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_Users_response_ok_status_code(string username, string password)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();

            //Assert.NotNull(registeredLocalization);

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var jsonParams = JsonConvert.SerializeObject(new LoginUiModel()
            {
                Login = username,
                Password = password
            });
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), requestContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthUiModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            // 4. read all - Fetch all

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);
            var fetchedUsers = await client.GetAsync(Get.GetUsersAsync());
            fetchedUsers.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One User
            var fetchedUsersResponse = await fetchedUsers.Content.ReadAsStringAsync();
            var fetchedUsersModel = JsonConvert.DeserializeObject(fetchedUsersResponse).ToString();
            var numUsers = JObject.Parse(fetchedUsersModel).GetValue("Value").Count();   // get the number of Users from the response        

            fetchedUsersModel.Should().NotBe(null);
            numUsers.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 1)]
        public async Task Get_Fetch_Single_User_ByID_shouldReturn_OK(string username, string password, long validID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();
            // Id = 1 => su
            //Assert.NotNull(registeredLocalization);

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var jsonParams = JsonConvert.SerializeObject(new LoginUiModel()
            {
                Login = username,
                Password = password
            });
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), requestContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthUiModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. read all - Fetch all


            var fetchedUser = await client.GetAsync(Get.GetUserByIdAsync(validID));
            fetchedUser.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One User
            var fetchedUserByIDResponse = await fetchedUser.Content.ReadAsStringAsync();
            var fetchedUserByIDModel = JsonConvert.DeserializeObject(fetchedUserByIDResponse);
            fetchedUserByIDModel.Should().NotBe(null);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_User_ByInvalidID_shouldReturn_BadRequest(string username, string password, long invalidID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();
            // Id = 1 => su
            //Assert.NotNull(registeredLocalization);

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var jsonParams = JsonConvert.SerializeObject(new LoginUiModel()
            {
                Login = username,
                Password = password
            });
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), requestContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthUiModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            //Assert.
            //If not valid ID check for BadRequest with reason being "User Id does not exist"
            var fetchedInvalidUser = await client.GetAsync(Get.GetUserByIdAsync(invalidID));
            fetchedInvalidUser.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidUserByIDResponse = await fetchedInvalidUser.Content.ReadAsStringAsync();
            string fetchedInvalidUserByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidUserByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidUserByIDMessage)["Messages"].First.ToString();
            Assert.Equal("ERROR_INVALID_USER_ID", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Fact (Skip = "Not ready yet")]  
        public async Task update_user_by_id_response_OK()
        {
            Startup();
        }

        [Theory(Skip = "Problem with deleteAsync and body parameters")]
        [InlineData(28)]
        public async Task deleteSoft_existingUser_response_ok_status_code(long validId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var jsonParams = JsonConvert.SerializeObject(new LoginUiModel()
            {
                Login = username,
                Password = password
            });
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), requestContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthUiModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create


            // 5. read one - get by id, previous created id


            var getResponse = await client.GetAsync(Get.GetUserByIdAsync(validId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);


            // 7. drop all/ Clean up the company created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteUserResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });

            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftUserAsync(validId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetUserByIdAsync(validId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        //(Skip = "Problem with deleteAsync and body parameters")
        [Theory(Skip= "Message: User with id: -1 has been deleted successfully.")]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingUser_response_badRequest(long invalidId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var jsonParams = JsonConvert.SerializeObject(new LoginUiModel()
            {
                Login = username,
                Password = password
            });
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), requestContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthUiModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create


            // 5. read one - get by id, previous created id


            var getResponse = await client.GetAsync(Get.GetUserByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the company created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteUserResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete,Delete.DeleteSoftUserAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
            //deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidUserByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidUserByIDMessage = JsonConvert.DeserializeObject(InvalidUserByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidUserByIDMessage)["Messages"].First.ToString();
            Assert.Equal("User Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetUserByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        //(Skip = "Problem with the response of the deleted User")
        [Theory(Skip = "Should create a user after deletion for following testing")]
        [InlineData(28)]
        public async Task deleteHard_existingUser_response_ok_status_code(long validId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var jsonParams = JsonConvert.SerializeObject(new LoginUiModel()
            {
                Login = username,
                Password = password
            });
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), requestContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthUiModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create // better approach, create a company so that it exists for sure

            // 5. check if the company exists

            var getResponse = await client.GetAsync(Get.GetUserByIdAsync(validId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardUserAsync(validId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetUserByIdAsync(validId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingUser_response_BadRequest(long invalidId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var jsonParams = JsonConvert.SerializeObject(new LoginUiModel()
            {
                Login = username,
                Password = password
            });
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), requestContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthUiModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create // better approach, create a company so that it exists for sure

            // 5. check if the company exists

            var getResponse = await client.GetAsync(Get.GetUserByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardUserAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidUserByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidUserByIDMessage = JsonConvert.DeserializeObject(InvalidUserByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidUserByIDMessage)["Messages"].First.ToString();
            Assert.Equal("User Id does not exist", reasonOfBadRequest);

        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }

}
