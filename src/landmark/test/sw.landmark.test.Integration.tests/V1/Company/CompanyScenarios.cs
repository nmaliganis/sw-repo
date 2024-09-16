using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.Companies;
using dottrack.asset.common.dtos.Vms.Companies;
using dottrack.asset.test.Integration.tests.Base;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dottrack.asset.test.Integration.tests.V1.Company
{
    [Collection("Scenarios")]
    public class CompanyScenarios
      : CompanyScenariosBase, IDisposable
    {

        [Theory]
        [ClassData(typeof(CreateCompanyTestData))]
        public async Task post_new_company_response_ok_status_code(CreateCompanyResourceParameters parameters)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();

            //Assert.NotNull(registeredLocalization);

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create
            var jsonParams = JsonConvert.SerializeObject(parameters);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostCompanyAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<CompanyCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetCompanyByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardCompanyAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetCompanyByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.

        }

        [Theory]
        [ClassData(typeof(CreateCompanyTestData))]
        public async Task delete_company_response_ok_status_code(CreateCompanyResourceParameters parameters)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();


            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create
            var jsonParams = JsonConvert.SerializeObject(parameters);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostCompanyAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<CompanyCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetCompanyByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardCompanyAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetCompanyByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [ClassData(typeof(UpdateCompanyTestData))]
        public async Task put_company_response_ok_status_code(
            CreateCompanyResourceParameters createParams, UpdateCompanyResourceParameters updateParams)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();


            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostCompanyAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<CompanyCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetCompanyByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<CompanyUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.Description.Should().Be(createParams.Description);

            // 6. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateCompanyAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<CompanyModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetCompanyByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<CompanyUiModel>>(modifiedContent);

            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);
            modifiedModel.Model.Description.Should().Be(updateParams.Description);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardCompanyAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetCompanyByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
