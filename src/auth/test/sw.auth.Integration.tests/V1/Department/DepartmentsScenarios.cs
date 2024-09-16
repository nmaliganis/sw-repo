using AutoMapper;
using dottrack.auth.Integration.tests.Base;
using dottrack.common.dtos.ResourceParameters.Departments;
using dottrack.common.dtos.Vms.Accounts;
using dottrack.common.dtos.Vms.Departments;
using emdot.infrastructure.BrokenRules;
using emdot.infrastructure.Paging;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using dottrack.auth.common.dtos.Vms.Departments;
using dottrack.auth.common.dtos.Vms.Users;
using Xunit;

namespace dottrack.auth.Integration.tests.V1.Department
{
    [Collection("Sequential")]
    public class DepartmentsScenarios
      : DepartmentsScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_Departments_response_ok_status_code(string username, string password)
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
            var fetchedDepartments = await client.GetAsync(Get.GetDepartmentsAsync());
            fetchedDepartments.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Department
            var fetchedDepartmentsResponse = await fetchedDepartments.Content.ReadAsStringAsync();
            var fetchedDepartmentsModel = JsonConvert.DeserializeObject(fetchedDepartmentsResponse).ToString();
            var numDepartments = JObject.Parse(fetchedDepartmentsModel).GetValue("Value").Count();   // get the number of Departments from the response        

            fetchedDepartmentsModel.Should().NotBe(null);
            numDepartments.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 3)]
        public async Task Get_Fetch_Single_Department_ByID(string username, string password, long validID)
        {
            Startup();
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
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. read all - Fetch all


            var fetchedDepartment = await client.GetAsync(Get.GetDepartmentByIdAsync(validID));
            fetchedDepartment.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Department
            var fetchedDepartmentByIDResponse = await fetchedDepartment.Content.ReadAsStringAsync();
            var fetchedDepartmentByIDModel = JsonConvert.DeserializeObject(fetchedDepartmentByIDResponse);
            fetchedDepartmentByIDModel.Should().NotBe(null);


            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_Department_ByInvalidID_shouldReturn_BadRequest(string username, string password, long invalidID)
        {
            Startup();
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
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            //If not valid ID check for BadRequest with reason being "Department Id does not exist"
            var fetchedInvalidDepartment = await client.GetAsync(Get.GetDepartmentByIdAsync(invalidID));
            fetchedInvalidDepartment.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidDepartmentByIDResponse = await fetchedInvalidDepartment.Content.ReadAsStringAsync();
            string fetchedInvalidDepartmentByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidDepartmentByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidDepartmentByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Department Id does not exist", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Fact]
        public async Task createExistingDepartment_response_BadRequest_departmentExists()
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";
            var existingDepartment = new CreateDepartmentResourceParameters
            {
                Name = "Dep_1",
                CodeErp = "100",
                Notes = "Dep notes"
            }; //Already existing Department

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
            var jsonCreateParams = JsonConvert.SerializeObject(existingDepartment);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDepartmentAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
            var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();

            //TO DO: IT SHOULD RETURN A MESSAGE OF KIND "ERROR_DEPARTMENT_ALREADY_EXISTS",
            // HOWEVER IT RETURNS: "ERROR_FETCH_COMPANY_NOT_EXISTS-"
            // even though a valid comoany ID is passed

            //Assert.Equal("ERROR_DEPARTMENT_ALREADY_EXISTS", reasonOfBadRequest); // IT RETURNS "ERROR_FETCH_COMPANY_NOT_EXISTS-"


            // 7. drop all
            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateDepartmentTestData))]
        public async Task create_department_response_ok_status_code(
        CreateDepartmentResourceParameters createParams)
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
            var postResponse = await client.PostAsync(Post.PostDepartmentAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_Department_ALREADY_EXISTS")) // IT SHOULD BE REFACTORED ACCORDING TO THE CORRECT MESSAGE -- see createExistingDepartment TEST
                {
                    createParams.Name = "TestComp" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostDepartmentAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the Department does not exist
            }


            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DepartmentCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<DepartmentUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.Notes.Should().Be(createParams.Notes);


            // 7. drop all/ Clean up the Department created
            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDepartmentAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }
        [Theory]
        [ClassData(typeof(UpdateDepartmentTestData))]
        public async Task update_Department_response_ok_status_code(
        CreateDepartmentResourceParameters createParams, UpdateDepartmentResourceParameters updateParams)
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
            var postResponse = await client.PostAsync(Post.PostDepartmentAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_Department_ALREADY_EXISTS")) // IT SHOULD BE REFACTORED ACCORDING TO THE CORRECT MESSAGE -- see createExistingDepartment TEST
                {
                    createParams.Name = "TestComp" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostDepartmentAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the Department does not exist
            }
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DepartmentCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<DepartmentUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.Notes.Should().Be(createParams.Notes);

            // 6. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateDepartmentAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<DepartmentModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<DepartmentUiModel>>(modifiedContent);

            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);
            modifiedModel.Model.Notes.Should().Be(updateParams.Notes);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDepartmentAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task update_nonExistingDepartment_response_BadRequest(long invalidId)
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

            var updateParams = new UpdateDepartmentResourceParameters
            {
                Name = "UpdatedTestDepartment2883982",
                CodeErp = "200",
                Notes = "Updated NOtes"
            };
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");

            var updateResponse = await client.PutAsync(Put.UpdateDepartmentAsync(invalidId), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidDepartmentByIDResponse = await updateResponse.Content.ReadAsStringAsync();
            string InvalidDepartmentByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmentByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDepartmentByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Department Id does not exist", reasonOfBadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [ClassData(typeof(CreateDepartmentTestData))]
        public async Task deleteSoft_existingDepartment_response_ok_status_code(CreateDepartmentResourceParameters createParams)
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

            // 4. create // better approach, create a department so that it exists for sure
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDepartmentAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DepartmentCreationUiModel>>(responseContent);

            // 6. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteDepartmentResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftDepartmentAsync(uiModel.Model.Id));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);


            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateDepartmentTestData))]
        public async Task deleteHard_existingDepartment_response_ok_status_code(CreateDepartmentResourceParameters createParams)
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

            // 4. create // better approach, create a department so that it exists for sure
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDepartmentAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DepartmentCreationUiModel>>(responseContent);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDepartmentAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingDepartment_response_badRequest(long invalidId)
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


            var getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the department created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteDepartmentResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftDepartmentAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
            //deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidDepartmentByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidDepartmentByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmentByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDepartmentByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Department Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingDepartment_response_BadRequest(long invalidId)
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

            // 4. create // better approach, create a department so that it exists for sure

            // 5. check if the department exists

            var getResponse = await client.GetAsync(Get.GetDepartmentByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDepartmentAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidDepartmnetByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidDepartmentByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmnetByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDepartmentByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Department Id does not exist", reasonOfBadRequest);

        }
        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
