using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.Vms.AssetCategories;
using dottrack.asset.Integration.tests.Base.Auth;
using dottrack.asset.Integration.tests.V1.AssetCategory;
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

namespace dottrack.asset.Integration.tests.V1.AssetCategory
{
    [Collection("Scenarios")]
    public class AssetCategoryScenarios
      : AssetCategoryScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_assetcategories_response_ok_status_code(string username, string password)
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
            var fetchedAssetCategories = await client.GetAsync(Get.GetAssetCategoriesAsync());
            fetchedAssetCategories.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One AssetCategory
            var fetchedAssetCategoriesResponse = await fetchedAssetCategories.Content.ReadAsStringAsync();
            var fetchedAssetCategoriesModel = JsonConvert.DeserializeObject(fetchedAssetCategoriesResponse).ToString();
            var numAssetCategories = JObject.Parse(fetchedAssetCategoriesModel).GetValue("Value").Count();   // get the number of asset categories from the response        

            fetchedAssetCategoriesModel.Should().NotBe(null);
            numAssetCategories.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 1)]
        public async Task Get_Fetch_Single_AssetCategory_ByID_shouldReurn_OK(string username, string password, long validID)
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


            var fetchedAssetCategory = await client.GetAsync(Get.GetAssetCategoryByIdAsync(validID));
            fetchedAssetCategory.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One AssetCategory
            var fetchedAssetCategoryByIDResponse = await fetchedAssetCategory.Content.ReadAsStringAsync();
            var fetchedAssetCategoryByIDModel = JsonConvert.DeserializeObject(fetchedAssetCategoryByIDResponse);
            fetchedAssetCategoryByIDModel.Should().NotBe(null);
            //Assert.

            // 6. drop all
            Dispose();
            //assetServer.

        }
        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_AssetCategory_ByinvalidID_shouldReturn_BadRequest(string username, string password, long invalidID)
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
            //If not valid ID check for BadRequest with reason being "AssetCategory Id does not exist"
            var fetchedInvalidAssetCategory = await client.GetAsync(Get.GetAssetCategoryByIdAsync(invalidID));
            fetchedInvalidAssetCategory.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidAssetCategoryByIDResponse = await fetchedInvalidAssetCategory.Content.ReadAsStringAsync();
            string fetchedInvalidAssetCategoryByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidAssetCategoryByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidAssetCategoryByIDMessage)["Messages"].First.ToString();
            Assert.Equal("AssetCategory Id does not exist", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Fact]
        public async Task createExistingAssetCategory_response_BadRequest_assetCategoryExists()
        {
            this.Startup();

            var newAssetCategory = new CreateAssetCategoryResourceParameters
            {
                Name = "TestCreation",
                CodeErp = "100",
                Params = "{Params:delegate}"
            }; //Already existing AssetCategory

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

            // 4. create the same asset category twice so it is sure that the asset category exists
            var jsonCreateParams = JsonConvert.SerializeObject(newAssetCategory);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostAssetCategoryAsync(), requestCreationContent);
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryCreationUiModel>>(responseContent);


            var jsonCreateParams2 = JsonConvert.SerializeObject(newAssetCategory);
            var requestCreationContent2 = new StringContent(jsonCreateParams2, Encoding.UTF8, "application/json");
            var postResponse2 = await client.PostAsync(Post.PostAssetCategoryAsync(), requestCreationContent2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var postResponeMessage = JsonConvert.DeserializeObject(await postResponse2.Content.ReadAsStringAsync()).ToString();
            var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
            Assert.Equal("ERROR_SENSORTYPE_ALREADY_EXISTS", reasonOfBadRequest);


            // 7. drop all
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardAssetCategoryAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateAssetCategoryTestData))]
        public async Task create_assetCategory_response_ok_status_code(
        CreateAssetCategoryResourceParameters createParams)
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
            var postResponse = await client.PostAsync(Post.PostAssetCategoryAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_SENSORTYPE_ALREADY_EXISTS"))
                {
                    createParams.Name = "TestST" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostAssetCategoryAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the assetCategory does not exist
            }


            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);



            // 7. drop all/ Clean up the assetCategory created
            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardAssetCategoryAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        //(Skip ="Not ready yet")
        [Theory]
        [ClassData(typeof(UpdateAssetCategoryTestData))]
        public async Task update_assetCategory_response_ok_status_code(
            CreateAssetCategoryResourceParameters createParams, UpdateAssetCategoryResourceParameters updateParams)
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
            var postResponse = await client.PostAsync(Post.PostAssetCategoryAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);

            // 6. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateAssetCategoryAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryUiModel>>(modifiedContent);

            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardAssetCategoryAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateAssetCategoryTestData))]
        public async Task deleteSoft_existingAssetCategory_response_ok_status_code(CreateAssetCategoryResourceParameters createParams)
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

            // 4. create // better approach, create a assetCategory so that it exists for sure
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostAssetCategoryAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryCreationUiModel>>(responseContent);


            // 6. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteAssetCategoryResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });

            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftAssetCategoryAsync(uiModel.Model.Id));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //Clean up
            var deleteHardResponse = await client.DeleteAsync(Delete.DeleteHardAssetCategoryAsync(uiModel.Model.Id));
            deleteHardResponse.StatusCode.Should().Be(HttpStatusCode.OK);


        }
        [Theory]
        [ClassData(typeof(CreateAssetCategoryTestData))]
        public async Task deleteHard_existingAssetCategory_response_ok_status_code(CreateAssetCategoryResourceParameters createParams)
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

            // 4. create // better approach, create a assetCategory so that it exists for sure
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostAssetCategoryAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryCreationUiModel>>(responseContent);


            // 6. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardAssetCategoryAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task update_nonExistingAssetCategory_response_BadRequest(long invalidId)
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

            var updateParams = new UpdateAssetCategoryResourceParameters
            {
                Name = "AssCat100Test",
                CodeErp = "100",
                Params = "{}"
            };
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");

            var updateResponse = await client.PutAsync(Put.UpdateAssetCategoryAsync(invalidId), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidAssetCategoryByIDResponse = await updateResponse.Content.ReadAsStringAsync();
            string InvalidAssetCategoryByIDMessage = JsonConvert.DeserializeObject(InvalidAssetCategoryByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidAssetCategoryByIDMessage)["Messages"].First.ToString();
            Assert.Equal("AssetCategory Id does not exist", reasonOfBadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingAssetCategory_response_badRequest(long invalidId)
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


            var getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the assetCategory created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteAssetCategoryResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftAssetCategoryAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
            //deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidAssetCategoryByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidAssetCategoryByIDMessage = JsonConvert.DeserializeObject(InvalidAssetCategoryByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidAssetCategoryByIDMessage)["Messages"].First.ToString();
            Assert.Equal("AssetCategory Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingAssetCategory_response_BadRequest(long invalidId)
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

            // 4. create // better approach, create a assetCategory so that it exists for sure

            // 5. check if the assetCategory exists

            var getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardAssetCategoryAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidDepartmnetByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidAssetCategoryByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmnetByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidAssetCategoryByIDMessage)["Messages"].First.ToString();
            Assert.Equal("AssetCategory Id does not exist", reasonOfBadRequest);

        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
