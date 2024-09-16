using AutoMapper;
using dottrack.auth.Integration.tests.Base;
using dottrack.common.dtos.Vms.Accounts;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using dottrack.auth.common.dtos.Vms.Users;
using Xunit;


namespace dottrack.auth.Integration.tests.V1.Message
{
    [Collection("Scenarios")]
    public class MessagesScenarios
      : MessagesScenariosBase, IDisposable
    {

        [Theory (Skip = "Not ready yet")]
        [ClassData(typeof(CreateMessageTestData))]
        public async Task CreatingMessage_ShouldReturnOKResponse(UserForRegistrationUiModel createParams)
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
            var postResponse = await client.PostAsync(Post.PostMessageAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_Department_ALREADY_EXISTS")) // IT SHOULD BE REFACTORED ACCORDING TO THE CORRECT MESSAGE -- see createExistingDepartment TEST
                {
                    createParams.Login = "TestUser" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostMessageAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the Department does not exist
            }


            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
     /*       var uiModel = JsonConvert.DeserializeObject<BusinessResult<UserCreationUiModel>>(responseContent);
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
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);*/
        }
        /*
        [Theory(Skip = "Not Ready yet")]
        [InlineData("su","su",1, -1)]
        public async Task FetchAccountsByID_response_ok_status_code(
            string username, string password, long validID, long invalidID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();
            var client = authServer.CreateIdempotentClient();


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
            // 4. create FKs
            var fetchedAccounts = await client.GetAsync(Get.GetAccountByIdAsync(validID));
            fetchedAccounts.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Company
            var fetchedCompanyByIDResponse = await fetchedAccounts.Content.ReadAsStringAsync();
            var fetchedCompanyByIDModel = JsonConvert.DeserializeObject(fetchedCompanyByIDResponse);
            fetchedCompanyByIDModel.Should().NotBe(null);
            //Assert.
            //If not valid ID check for BadRequest with reason being "Company Id does not exist"
            var fetchedInvalidAccount = await client.GetAsync(Get.GetAccountByIdAsync(invalidID));
            fetchedInvalidAccount.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidAccountByIDResponse = await fetchedInvalidAccount.Content.ReadAsStringAsync();
            string fetchedInvalidAccountByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidAccountByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidAccountByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Company Id does not exist", reasonOfBadRequest);

            // 9. drop all
            //assetServer.
        }
        /*
        [Theory]
        [ClassData(typeof(UpdateAccountTestData))]
        public async Task put_Account_response_ok_status_code(
            CreateAccountResourceParameters createParams,
            UpdateAccountResourceParameters updateParams,
            CreateAssetCategoryResourceParameters assetCatParams,
            CreateCompanyResourceParameters companyParams)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();


            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create FKs
            var acId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            createParams.AssetCategoryId = acId;
            updateParams.AssetCategoryId = acId;

            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            createParams.CompanyId = companyId;
            updateParams.CompanyId = companyId;

            // 5. create
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostAccountAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<AccountCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetAccountByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<AccountUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.Image.Should().Be(createParams.Image);
            newModel.Model.Description.Should().Be(createParams.Description);
            newModel.Model.IsVisible.Should().Be(createParams.IsVisible);
            newModel.Model.Level.Should().Be(createParams.Level);
            newModel.Model.Latitude.Should().Be(createParams.Latitude);
            newModel.Model.Longitude.Should().Be(createParams.Longitude);
            newModel.Model.TimeToFull.Should().Be(createParams.TimeToFull);
            newModel.Model.LastServicedDate.ToString("G").Should().Be(createParams.LastServicedDate.ToString("G"));
            newModel.Model.Status.Should().Be(createParams.Status);
            newModel.Model.MandatoryPickupDate.ToString("G").Should().Be(createParams.MandatoryPickupDate.ToString("G"));
            newModel.Model.MandatoryPickupActive.Should().Be(createParams.MandatoryPickupActive);
            newModel.Model.Capacity.Should().Be(createParams.Capacity);
            newModel.Model.WasteType.Should().Be(createParams.WasteType);
            newModel.Model.Material.Should().Be(createParams.Material);

            // 7. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateAccountAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<AccountModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetAccountByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<AccountUiModel>>(modifiedContent);

            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);
            modifiedModel.Model.Image.Should().Be(updateParams.Image);
            modifiedModel.Model.Description.Should().Be(updateParams.Description);
            modifiedModel.Model.IsVisible.Should().Be(updateParams.IsVisible);
            modifiedModel.Model.Level.Should().Be(updateParams.Level);
            modifiedModel.Model.Latitude.Should().Be(updateParams.Latitude);
            modifiedModel.Model.Longitude.Should().Be(updateParams.Longitude);
            modifiedModel.Model.TimeToFull.Should().Be(updateParams.TimeToFull);
            modifiedModel.Model.LastServicedDate.ToString("G").Should().Be(updateParams.LastServicedDate.ToString("G"));
            modifiedModel.Model.Status.Should().Be(updateParams.Status);
            modifiedModel.Model.MandatoryPickupDate.ToString("G").Should().Be(updateParams.MandatoryPickupDate.ToString("G"));
            modifiedModel.Model.MandatoryPickupActive.Should().Be(updateParams.MandatoryPickupActive);
            modifiedModel.Model.Capacity.Should().Be(updateParams.Capacity);
            modifiedModel.Model.WasteType.Should().Be(updateParams.WasteType);
            modifiedModel.Model.Material.Should().Be(updateParams.Material);



            // 8. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardAccountAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetAccountByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 9. delete FKs
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            // 10. drop all
            //assetServer.
        }
        */
        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
