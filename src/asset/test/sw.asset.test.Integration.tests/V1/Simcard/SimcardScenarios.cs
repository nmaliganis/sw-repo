using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.Simcards;
using dottrack.asset.common.dtos.Vms.Simcards;
using dottrack.asset.Integration.tests.Base.Auth;
using dottrack.asset.Integration.tests.V1.Simcard;
using dottrack.asset.test.Integration.tests.Base;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Microsoft.Azure.Amqp.Framing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dottrack.asset.Integration.tests.V1.Simcard
{
    [Collection("Scenarios")]
    public class SimcardScenarios
      : SimcardScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_simcards_response_ok_status_code(string username, string password)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);
            var fetchedSimcards = await client.GetAsync(Get.GetSimcardsAsync());
            fetchedSimcards.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Simcard
            var fetchedSimcardsResponse = await fetchedSimcards.Content.ReadAsStringAsync();
            var fetchedSimcardsModel = JsonConvert.DeserializeObject(fetchedSimcardsResponse).ToString();
            var numSimcards = JObject.Parse(fetchedSimcardsModel).GetValue("Value").Count();   // get the number of simcards from the response        

            fetchedSimcardsModel.Should().NotBe(null);
            numSimcards.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 4)]
        public async Task Get_Fetch_Single_Simcard_ByID_shouldReurn_OK(string username, string password, long validID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. read all - Fetch all


            var fetchedSimcard = await client.GetAsync(Get.GetSimcardByIdAsync(validID));
            fetchedSimcard.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Simcard
            var fetchedSimcardByIDResponse = await fetchedSimcard.Content.ReadAsStringAsync();
            var fetchedSimcardByIDModel = JsonConvert.DeserializeObject(fetchedSimcardByIDResponse);
            fetchedSimcardByIDModel.Should().NotBe(null);
            //Assert.

            // 6. drop all
            Dispose();
            //assetServer.

        }
        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_Simcard_ByinvalidID_shouldReturn_BadRequest(string username, string password, long invalidID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            //Assert.
            //If not valid ID check for BadRequest with reason being "Simcard Id does not exist"
            var fetchedInvalidSimcard = await client.GetAsync(Get.GetSimcardByIdAsync(invalidID));
            fetchedInvalidSimcard.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidSimcardByIDResponse = await fetchedInvalidSimcard.Content.ReadAsStringAsync();
            string fetchedInvalidSimcardByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidSimcardByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidSimcardByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Simcard Id does not exist", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Fact]
        public async Task createExistingSimcard_response_BadRequest_simcardExists()
        {
            this.Startup();
            var rnd = new Random();
            var newSimcard = new CreateSimcardResourceParameters
            {
                Number = "69" + rnd.Next(10000000, 99999999),
                CodeErp = "100"
            }; //Already existing Simcard

            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

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
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create the same simcard twice so it is sure that the simcard exists
            var jsonCreateParams = JsonConvert.SerializeObject(newSimcard);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSimcardAsync(), requestCreationContent);
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SimcardCreationUiModel>>(responseContent);


            var jsonCreateParams2 = JsonConvert.SerializeObject(newSimcard);
            var requestCreationContent2 = new StringContent(jsonCreateParams2, Encoding.UTF8, "application/json");
            var postResponse2 = await client.PostAsync(Post.PostSimcardAsync(), requestCreationContent2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var postResponeMessage = JsonConvert.DeserializeObject(await postResponse2.Content.ReadAsStringAsync()).ToString();
            var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
            Assert.Equal("ERROR_SIMCARD_ALREADY_EXISTS", reasonOfBadRequest);


            // 7. drop all
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSimcardAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateSimcardTestData))]
        public async Task create_simcard_response_ok_status_code(
        CreateSimcardResourceParameters createParams)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

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
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSimcardAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_SENSORTYPE_ALREADY_EXISTS"))
                {
                    createParams.Number = "69" + rnd.Next(10000000,99999999).ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostSimcardAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the simcard does not exist
            }


            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SimcardCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<SimcardUiModel>>(newContent);

            newModel.Model.Number.Should().Be(createParams.Number);

            // 7. drop all/ Clean up the simcard created
            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSimcardAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        //(Skip ="Not ready yet")
        [Theory]
        [ClassData(typeof(UpdateSimcardTestData))]
        public async Task update_simcard_response_ok_status_code(
            CreateSimcardResourceParameters createParams, UpdateSimcardResourceParameters updateParams)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

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
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSimcardAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SimcardCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<SimcardUiModel>>(newContent);

            newModel.Model.Number.Should().Be(createParams.Number);

            // 6. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateSimcardAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<SimcardModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<SimcardUiModel>>(modifiedContent);

            modifiedModel.Model.Number.Should().Be(updateParams.Number);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSimcardAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateSimcardTestData))]
        public async Task deleteSoft_existingSimcard_response_ok_status_code(CreateSimcardResourceParameters createParams)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

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
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create // better approach, create a simcard so that it exists for sure
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSimcardAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SimcardCreationUiModel>>(responseContent);


            // 6. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteSimcardResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });

            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftSimcardAsync(uiModel.Model.Id));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //Clean up
            var deleteHardResponse = await client.DeleteAsync(Delete.DeleteHardSimcardAsync(uiModel.Model.Id));
            deleteHardResponse.StatusCode.Should().Be(HttpStatusCode.OK);


        }
        [Theory]
        [ClassData(typeof(CreateSimcardTestData))]
        public async Task deleteHard_existingSimcard_response_ok_status_code(CreateSimcardResourceParameters createParams)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

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
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create // better approach, create a simcard so that it exists for sure
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSimcardAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SimcardCreationUiModel>>(responseContent);


            // 6. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSimcardAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task update_nonExistingSimcard_response_BadRequest(long invalidId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

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
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create


            // 5. read one - get by id, previous created id



            // 6. update newValue

            var updateParams = new UpdateSimcardResourceParameters
            {
                Number = "6912312312",
                CodeErp = "100",
                Notes = "should not exist"
            };
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");

            var updateResponse = await client.PutAsync(Put.UpdateSimcardAsync(invalidId), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidSimcardByIDResponse = await updateResponse.Content.ReadAsStringAsync();
            string InvalidSimcardByIDMessage = JsonConvert.DeserializeObject(InvalidSimcardByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidSimcardByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Simcard Id does not exist", reasonOfBadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingSimcard_response_badRequest(long invalidId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

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
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create


            // 5. read one - get by id, previous created id


            var getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the simcard created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteSimcardResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftSimcardAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
            //deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidSimcardByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidSimcardByIDMessage = JsonConvert.DeserializeObject(InvalidSimcardByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidSimcardByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Simcard Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingSimcard_response_BadRequest(long invalidId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

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
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create // better approach, create a simcard so that it exists for sure

            // 5. check if the simcard exists

            var getResponse = await client.GetAsync(Get.GetSimcardByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSimcardAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidDepartmnetByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidSimcardByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmnetByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidSimcardByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Simcard Id does not exist", reasonOfBadRequest);

        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
