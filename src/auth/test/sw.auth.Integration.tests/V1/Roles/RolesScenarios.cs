using AutoMapper;
using dottrack.auth.Integration.tests.Base;
using dottrack.auth.Integration.tests.V1.Roles;
using dottrack.common.dtos.ResourceParameters.Roles;
using dottrack.common.dtos.Vms.Accounts;
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
using dottrack.auth.common.dtos.Vms.Roles;
using dottrack.auth.common.dtos.Vms.Users;
using Xunit;

namespace dottrack.auth.Integration.tests.V1.Role
{
    [Collection("Sequential")]
    public class RolesScenarios
      : RolesScenariosBase, IDisposable
    {

        [Theory]
        [ClassData(typeof(CreateRoleTestData))]
        public async Task create_role_response_ok_status_code(CreateRoleResourceParameters createParams)
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
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostRoleAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_ROLE_ALREADY_EXISTS"))
                {
                    createParams.Name = "TestComp" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostRoleAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the role does not exist
            }


            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<RoleCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetRoleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<RoleUiModel>>(newContent);




            // 7. drop all/ Clean up the role created
            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardRoleAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetRoleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Fact]
        public async Task createExistingRole_response_BadRequest_roleExists()
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";
            var newRole = new CreateRoleResourceParameters
            {
                Name = "test_role"
            }; //Already existing Role

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
            var jsonCreateParams = JsonConvert.SerializeObject(newRole);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostRoleAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
            var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
            Assert.Equal("ERROR_ROLE_ALREADY_EXISTS", reasonOfBadRequest);


            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_Roles_response_ok_status_code(string username, string password)
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
            var fetchedRoles = await client.GetAsync(Get.GetRolesAsync());
            fetchedRoles.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Role
            var fetchedRolesResponse = await fetchedRoles.Content.ReadAsStringAsync();
            var fetchedRolesModel = JsonConvert.DeserializeObject(fetchedRolesResponse).ToString();
            var numRoles = JObject.Parse(fetchedRolesModel).GetValue("Value").Count();   // get the number of Roles from the response        

            fetchedRolesModel.Should().NotBe(null);
            numRoles.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 2)]
        public async Task Get_Fetch_Single_Role_ByID_ShouldRetunr_OK(string username, string password, long validID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();
            // Id = 2 => ADMIN
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


            var fetchedRole = await client.GetAsync(Get.GetRoleByIdAsync(validID));
            fetchedRole.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Role
            var fetchedRoleByIDResponse = await fetchedRole.Content.ReadAsStringAsync();
            var fetchedRoleByIDModel = JsonConvert.DeserializeObject(fetchedRoleByIDResponse);
            fetchedRoleByIDModel.Should().NotBe(null);
            //Assert.
            //If not valid ID check for BadRequest with reason being "Role Id does not exist"? Not working at the moment

            // 6. drop all
            Dispose();
            //assetServer.

        }
        //(Skip = "Not working, some problem with the Response of invalid ID")
        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_Role_ByID_ShouldRetunrBadRequest(string username, string password, long invalidID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();
            // Id = 2 => ADMIN
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

            //Assert.
            //If not valid ID check for BadRequest with reason being "Role Id does not exist"? Not working at the moment
            var fetchedInvalidRole = await client.GetAsync(Get.GetRoleByIdAsync(invalidID));
            fetchedInvalidRole.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidRoleByIDResponse = await fetchedInvalidRole.Content.ReadAsStringAsync();
            string fetchedInvalidRoleByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidRoleByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidRoleByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Role Id does not exist", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Theory]
        [ClassData(typeof(CreateRoleTestData))]
        public async Task deleteSoft_existingRole_response_ok_status_code(CreateRoleResourceParameters createParams)
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

            // Create a role

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostRoleAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<RoleCreationUiModel>>(responseContent);


            // 7. drop all/ Clean up the role created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteRoleResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftRoleAsync(uiModel.Model.Id));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            //var deleteResponse = await client.DeleteAsync(Delete.DeleteSoftRoleAsync(validId), deleteRequestContent); //problem with DeleteAsync and body parameters
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetRoleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingRole_response_badRequest(long invalidId)
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


            var getResponse = await client.GetAsync(Get.GetRoleByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the role created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteRoleResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftRoleAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidRoleByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidRoleByIDMessage = JsonConvert.DeserializeObject(InvalidRoleByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidRoleByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Role Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetRoleByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [ClassData(typeof(CreateRoleTestData))]
        public async Task deleteHard_existingRole_response_ok_status_code(CreateRoleResourceParameters createParams)
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

            // 4. create // better approach, create a role so that it exists for sure

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostRoleAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<RoleCreationUiModel>>(responseContent);


            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardRoleAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetRoleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingRole_response_BadRequest(long invalidId)
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

            // 4. create // better approach, create a role so that it exists for sure


            // 5. check if the role exists

            var getResponse = await client.GetAsync(Get.GetRoleByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardRoleAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidRoleByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidRoleByIDMessage = JsonConvert.DeserializeObject(InvalidRoleByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidRoleByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Role Id does not exist", reasonOfBadRequest);

        }

        //(Skip ="Not ready yet")
        [Theory]
        [ClassData(typeof(UpdateRoleTestData))]
        public async Task update_role_response_ok_status_code(
            CreateRoleResourceParameters createParams, UpdateRoleResourceParameters updateParams)
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
            }); ;
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await client.PostAsync(PostJwt.PostLogin(), requestContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthUiModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostRoleAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<RoleCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetRoleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<RoleUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);

            // 6. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateRoleAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<RoleModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetRoleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<RoleUiModel>>(modifiedContent);

            modifiedModel.Model.Name.Should().Be(updateParams.Name);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardRoleAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetRoleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);



            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task update_nonExistingRole_response_BadRequest(long invalidId)
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
            }); ;
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



            // 6. update newValue

            var updateParams = new UpdateRoleResourceParameters
            {
                Name = "UpdatedTestRole2883"
            };
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");

            var updateResponse = await client.PutAsync(Put.UpdateRoleAsync(invalidId), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidRoleByIDResponse = await updateResponse.Content.ReadAsStringAsync();
            string InvalidRoleByIDMessage = JsonConvert.DeserializeObject(InvalidRoleByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidRoleByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Role Id does not exist", reasonOfBadRequest);

            // 7. drop all
            //assetServer.
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}

