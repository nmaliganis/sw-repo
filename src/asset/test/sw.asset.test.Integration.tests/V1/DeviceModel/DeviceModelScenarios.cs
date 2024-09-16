using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using dottrack.asset.common.dtos.Vms.DeviceModels;
using dottrack.asset.Integration.tests.Base.Auth;
using dottrack.asset.test.Integration.tests.Base;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dottrack.asset.Integration.tests.V1.DeviceModel
{
    [Collection("Scenarios")]
    public class DeviceModelScenarios
      : DeviceModelScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_devicemodels_response_ok_status_code(string username, string password)
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
            var fetchedDeviceModels = await client.GetAsync(Get.GetDeviceModelsAsync());
            fetchedDeviceModels.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One DeviceModel
            var fetchedDeviceModelsResponse = await fetchedDeviceModels.Content.ReadAsStringAsync();
            var fetchedDeviceModelsModel = JsonConvert.DeserializeObject(fetchedDeviceModelsResponse).ToString();
            var numDeviceModels = JObject.Parse(fetchedDeviceModelsModel).GetValue("Value").Count();   // get the number of device models from the response        

            fetchedDeviceModelsModel.Should().NotBe(null);
            numDeviceModels.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 1)]
        public async Task Get_Fetch_Single_DeviceModel_ByID_shouldReurn_OK(string username, string password, long validID)
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


            var fetchedDeviceModel = await client.GetAsync(Get.GetDeviceModelByIdAsync(validID));
            fetchedDeviceModel.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One DeviceModel
            var fetchedDeviceModelByIDResponse = await fetchedDeviceModel.Content.ReadAsStringAsync();
            var fetchedDeviceModelByIDModel = JsonConvert.DeserializeObject(fetchedDeviceModelByIDResponse);
            fetchedDeviceModelByIDModel.Should().NotBe(null);
            //Assert.

            // 6. drop all
            Dispose();
            //assetServer.

        }
        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_DeviceModel_ByinvalidID_shouldReturn_BadRequest(string username, string password, long invalidID)
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
            //If not valid ID check for BadRequest with reason being "DeviceModel Id does not exist"
            var fetchedInvalidDeviceModel = await client.GetAsync(Get.GetDeviceModelByIdAsync(invalidID));
            fetchedInvalidDeviceModel.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidDeviceModelByIDResponse = await fetchedInvalidDeviceModel.Content.ReadAsStringAsync();
            string fetchedInvalidDeviceModelByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidDeviceModelByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidDeviceModelByIDMessage)["Messages"].First.ToString();
            Assert.Equal("DeviceModel Id does not exist", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        
        [Fact (Skip = "Not clear if device model name should be unique")]
        public async Task createExistingDeviceModel_response_BadRequest_deviceModelExists()
        {
            this.Startup();

            var newDeviceModel = new CreateDeviceModelResourceParameters
            {
                Name = "DM-Test",
                CodeErp = "1000",
                CodeName = "DM_1000",
                Enabled = true
            }; //Already existing DeviceModel

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

            // 4. create the same Device Model twice so it is sure that the device model exists
            var jsonCreateParams = JsonConvert.SerializeObject(newDeviceModel);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestCreationContent);
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelCreationUiModel>>(responseContent);


            var jsonCreateParams2 = JsonConvert.SerializeObject(newDeviceModel);
            var requestCreationContent2 = new StringContent(jsonCreateParams2, Encoding.UTF8, "application/json");
            var postResponse2 = await client.PostAsync(Post.PostDeviceModelAsync(), requestCreationContent2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var postResponeMessage = JsonConvert.DeserializeObject(await postResponse2.Content.ReadAsStringAsync()).ToString();
            var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
            Assert.Equal("ERROR_DEVICEMODEL_ALREADY_EXISTS", reasonOfBadRequest);


            // 7. drop all
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceModelAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateDeviceModelTestData))]
        public async Task create_deviceModel_response_ok_status_code(
        CreateDeviceModelResourceParameters createParams)
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
            var postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_DEVICEMODEL_ALREADY_EXISTS"))
                {
                    createParams.Name = "TestDM" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the deviceModel does not exist
            }


            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.CodeName.Should().Be(createParams.CodeName);


            // 7. drop all/ Clean up the deviceModel created
            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceModelAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        //(Skip ="Not ready yet")
        [Theory]
        [ClassData(typeof(UpdateDeviceModelTestData))]
        public async Task update_deviceModel_response_ok_status_code(
            CreateDeviceModelResourceParameters createParams, UpdateDeviceModelResourceParameters updateParams)
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
            var postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.CodeName.Should().Be(createParams.CodeName);

            // 6. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateDeviceModelAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelUiModel>>(modifiedContent);

            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);
            modifiedModel.Model.CodeName.Should().Be(updateParams.CodeName);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceModelAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateDeviceModelTestData))]
        public async Task deleteSoft_existingDeviceModel_response_ok_status_code(CreateDeviceModelResourceParameters createParams)
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

            // 4. create // better approach, create a deviceModel so that it exists for sure
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelCreationUiModel>>(responseContent);


            // 6. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteDeviceModelResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });

            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftDeviceModelAsync(uiModel.Model.Id));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //Clean up
            var deleteHardResponse = await client.DeleteAsync(Delete.DeleteHardDeviceModelAsync(uiModel.Model.Id));
            deleteHardResponse.StatusCode.Should().Be(HttpStatusCode.OK);


        }
        [Theory]
        [ClassData(typeof(CreateDeviceModelTestData))]
        public async Task deleteHard_existingDeviceModel_response_ok_status_code(CreateDeviceModelResourceParameters createParams)
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

            // 4. create // better approach, create a deviceModel so that it exists for sure
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelCreationUiModel>>(responseContent);


            // 6. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceModelAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task update_nonExistingDeviceModel_response_BadRequest(long invalidId)
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

            var updateParams = new UpdateDeviceModelResourceParameters
            {
                Name = "UpdatedTestDeviceModel2883982",
                CodeErp = "200",
                CodeName = "200"
            };
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");

            var updateResponse = await client.PutAsync(Put.UpdateDeviceModelAsync(invalidId), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidDeviceModelByIDResponse = await updateResponse.Content.ReadAsStringAsync();
            string InvalidDeviceModelByIDMessage = JsonConvert.DeserializeObject(InvalidDeviceModelByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDeviceModelByIDMessage)["Messages"].First.ToString();
            Assert.Equal("DeviceModel Id does not exist", reasonOfBadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingDeviceModel_response_badRequest(long invalidId)
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


            var getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the deviceModel created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteDeviceModelResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftDeviceModelAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
            //deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidDeviceModelByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidDeviceModelByIDMessage = JsonConvert.DeserializeObject(InvalidDeviceModelByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDeviceModelByIDMessage)["Messages"].First.ToString();
            Assert.Equal("DeviceModel Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingDeviceModel_response_BadRequest(long invalidId)
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

            // 4. create // better approach, create a deviceModel so that it exists for sure

            // 5. check if the deviceModel exists

            var getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceModelAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidDepartmnetByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidDeviceModelByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmnetByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDeviceModelByIDMessage)["Messages"].First.ToString();
            Assert.Equal("DeviceModel Id does not exist", reasonOfBadRequest);

        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
