using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Containers;
using dottrack.asset.common.dtos.ResourceParameters.Companies;
using dottrack.asset.common.dtos.Vms.Assets.Containers;
using dottrack.asset.test.Integration.tests.Base;
using dottrack.asset.test.Integration.tests.V1.Container.ForeignKeySetups;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dottrack.asset.test.Integration.tests.V1.Container
{
    [Collection("Scenarios")]
    public class ContainerScenarios
      : ContainerScenariosBase, IDisposable
    {

        [Theory]
        [ClassData(typeof(CreateContainerTestData))]
        public async Task post_new_container_response_ok_status_code(
            CreateContainerResourceParameters containerParams,
            CreateAssetCategoryResourceParameters assetCatParams,
            CreateCompanyResourceParameters companyParams)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();

            //Assert.NotNull(registeredasset);

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create FKs
            var acId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            containerParams.AssetCategoryId = acId;

            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.CompanyId = companyId;

            // 5. create
            var jsonParams = JsonConvert.SerializeObject(containerParams);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 8. delete FKs
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            // 9. drop all
            //assetServer.

        }

        [Theory]
        [ClassData(typeof(CreateContainerTestData))]
        public async Task delete_container_response_ok_status_code(
            CreateContainerResourceParameters containerParams,
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
            containerParams.AssetCategoryId = acId;

            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.CompanyId = companyId;

            // 5. create
            var jsonParams = JsonConvert.SerializeObject(containerParams);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 8. delete FKs
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            // 9. drop all
            //assetServer.
        }

        [Theory]
        [ClassData(typeof(UpdateContainerTestData))]
        public async Task put_container_response_ok_status_code(
            CreateContainerResourceParameters createParams,
            UpdateContainerResourceParameters updateParams,
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
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<ContainerUiModel>>(newContent);

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
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetContainerByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 9. delete FKs
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            // 10. drop all
            //assetServer.
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
