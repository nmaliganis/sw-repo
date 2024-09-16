using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Containers;
using dottrack.asset.common.dtos.ResourceParameters.Companies;
using dottrack.asset.common.dtos.Vms.Assets.Containers;
using dottrack.asset.Integration.tests.Base.Auth;
using dottrack.asset.Integration.tests.V1.Container.ForeignKeySetups;
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

namespace dottrack.asset.Integration.tests.V1.Container
{
    [Collection("Scenarios")]
    public class ContainerScenarios
      : ContainerScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_containers_response_ok_status_code(string username, string password)
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
            var fetchedContainers = await client.GetAsync(Get.GetContainersAsync());
            fetchedContainers.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Container
            var fetchedContainersResponse = await fetchedContainers.Content.ReadAsStringAsync();
            var fetchedContainersModel = JsonConvert.DeserializeObject(fetchedContainersResponse).ToString();
            var numContainers = JObject.Parse(fetchedContainersModel).GetValue("Value").Count();   // get the number of containers from the response        

            fetchedContainersModel.Should().NotBe(null);
            numContainers.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 1)]
        public async Task Get_Fetch_Single_Container_ByID_shouldReurn_OK(string username, string password, long validID)
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


            var fetchedContainer = await client.GetAsync(Get.GetContainerByIdAsync(validID));
            fetchedContainer.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Container
            var fetchedContainerByIDResponse = await fetchedContainer.Content.ReadAsStringAsync();
            var fetchedContainerByIDModel = JsonConvert.DeserializeObject(fetchedContainerByIDResponse);
            fetchedContainerByIDModel.Should().NotBe(null);
            //Assert.

            // 6. drop all
            Dispose();
            //assetServer.

        }
        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_Container_ByinvalidID_shouldReturn_BadRequest(string username, string password, long invalidID)
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
            //If not valid ID check for BadRequest with reason being "Container Id does not exist"
            var fetchedInvalidContainer = await client.GetAsync(Get.GetContainerByIdAsync(invalidID));
            fetchedInvalidContainer.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidContainerByIDResponse = await fetchedInvalidContainer.Content.ReadAsStringAsync();
            string fetchedInvalidContainerByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidContainerByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidContainerByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Container Id does not exist", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Fact]
        public async Task createExistingContainer_response_BadRequest_containerExists()
        {
            this.Startup();
            var newContainer = new CreateContainerResourceParameters
            {
                Name = "Cont1",
                CodeErp = "10000",
                Image = "img",
                Description = "descr",
                IsVisible = true,
                Level = 1,
                Latitude = 42.222,
                Longitude = 22.222,
                TimeToFull = 0,
                LastServicedDate = DateTime.Now,
                Status = 1,
                MandatoryPickupDate = DateTime.Now,
                MandatoryPickupActive = true,
                Capacity = 1,
                WasteType = 1,
                Material = 1,
                CompanyId = 1,
                AssetCategoryId = 1,
            }; //Already existing Container

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

            // 4. create the same container twice so it is sure that the container exists
            var jsonCreateParams = JsonConvert.SerializeObject(newContainer);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestCreationContent);
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);


            var jsonCreateParams2 = JsonConvert.SerializeObject(newContainer);
            var requestCreationContent2 = new StringContent(jsonCreateParams2, Encoding.UTF8, "application/json");
            var postResponse2 = await client.PostAsync(Post.PostContainerAsync(), requestCreationContent2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var postResponeMessage = JsonConvert.DeserializeObject(await postResponse2.Content.ReadAsStringAsync()).ToString();
            var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
            Assert.Equal("ERROR_CONTAINER_ALREADY_EXISTS", reasonOfBadRequest);


            // 7. drop all
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateContainerTestData))]
        public async Task create_container_response_ok_status_code(
        CreateContainerResourceParameters createParams, 
        CreateAssetCategoryResourceParameters assetCatParams,
        CreateCompanyResourceParameters companyParams)
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

            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            createParams.CompanyId = companyId;
            var assetCategoryId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            createParams.AssetCategoryId = assetCategoryId;

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_CONTAINER_ALREADY_EXISTS"))
                {
                    createParams.Name = "TestContainer" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostContainerAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the container does not exist
            }


            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<ContainerUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.Latitude.Should().Be(createParams.Latitude);
            newModel.Model.Longitude.Should().Be(createParams.Longitude);


            // 7. drop all/ Clean up the container created
            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            await CompanySetup.DeleteCompany(client, companyId);
            await AssetCategorySetup.DeleteAssetCategory(client, assetCategoryId);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        //(Skip ="Not ready yet")
        [Theory]
        [ClassData(typeof(UpdateContainerTestData))]
        public async Task update_container_response_ok_status_code(
            CreateContainerResourceParameters createParams,
            UpdateContainerResourceParameters updateParams,
            CreateAssetCategoryResourceParameters assetCatParams, 
            CreateCompanyResourceParameters companyParams
            )
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
            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            createParams.CompanyId = companyId;
            var assetCategoryId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            createParams.AssetCategoryId = assetCategoryId;

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<ContainerUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.Latitude.Should().Be(createParams.Latitude);
            newModel.Model.Longitude.Should().Be(createParams.Longitude);

            // 6. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateContainerAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<ContainerModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<ContainerUiModel>>(modifiedContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.Latitude.Should().Be(createParams.Latitude);
            newModel.Model.Longitude.Should().Be(createParams.Longitude);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);



            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.

            await CompanySetup.DeleteCompany(client, companyId);
            await AssetCategorySetup.DeleteAssetCategory(client, assetCategoryId);
        }
        [Theory]
        [ClassData(typeof(CreateContainerTestData))]
        public async Task deleteSoft_existingContainer_response_ok_status_code(
            CreateContainerResourceParameters createParams, 
            CreateAssetCategoryResourceParameters assetCatParams,
            CreateCompanyResourceParameters companyParams)
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

            // 4. create // better approach, create a container so that it exists for sure

            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            createParams.CompanyId = companyId;
            var assetCategoryId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            createParams.AssetCategoryId = assetCategoryId;

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);


            // 6. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteContainerResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });

            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftContainerAsync(uiModel.Model.Id));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //Clean up
            var deleteHardResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(uiModel.Model.Id));
            deleteHardResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            await CompanySetup.DeleteCompany(client, companyId);
            await AssetCategorySetup.DeleteAssetCategory(client, assetCategoryId);


        }
        [Theory]
        [ClassData(typeof(CreateContainerTestData))]
        public async Task deleteHard_existingContainer_response_ok_status_code(
            CreateContainerResourceParameters createParams,
            CreateAssetCategoryResourceParameters assetCatParams,
            CreateCompanyResourceParameters companyParams
            
            )
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

            // 4. create // better approach, create a container so that it exists for sure

            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            createParams.CompanyId = companyId;
            var assetCategoryId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            createParams.AssetCategoryId = assetCategoryId;

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);


            // 6. delete 

            await CompanySetup.DeleteCompany(client, companyId);
            await AssetCategorySetup.DeleteAssetCategory(client, assetCategoryId);

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);




            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task update_nonExistingContainer_response_BadRequest(long invalidId)
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

            var updateParams = new UpdateContainerResourceParameters
            {
                Name = "2883982",
                Latitude = 42.111,
                Longitude = 28.111
            };
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");

            var updateResponse = await client.PutAsync(Put.UpdateContainerAsync(invalidId), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidContainerByIDResponse = await updateResponse.Content.ReadAsStringAsync();
            string InvalidContainerByIDMessage = JsonConvert.DeserializeObject(InvalidContainerByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidContainerByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Container Id does not exist", reasonOfBadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingContainer_response_badRequest(long invalidId)
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


            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the container created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteContainerResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftContainerAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
            //deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidContainerByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidContainerByIDMessage = JsonConvert.DeserializeObject(InvalidContainerByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidContainerByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Container Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetContainerByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingContainer_response_BadRequest(long invalidId)
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

            // 4. create // better approach, create a container so that it exists for sure

            // 5. check if the container exists

            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidDepartmnetByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidContainerByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmnetByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidContainerByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Container Id does not exist", reasonOfBadRequest);

        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
