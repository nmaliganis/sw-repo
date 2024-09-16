using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Vehicles;
using dottrack.asset.common.dtos.ResourceParameters.Companies;
using dottrack.asset.common.dtos.Vms.Assets.Vehicles;
using dottrack.asset.test.Integration.tests.Base;
using dottrack.asset.test.Integration.tests.V1.Vehicle.ForeignKeySetups;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dottrack.asset.test.Integration.tests.V1.Vehicle
{
    [Collection("Scenarios")]
    public class VehicleScenarios
      : VehicleScenariosBase, IDisposable
    {

        [Theory]
        [ClassData(typeof(CreateVehicleTestData))]
        public async Task post_new_vehicle_response_ok_status_code(
            CreateVehicleResourceParameters createParams,
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
            createParams.AssetCategoryId = acId;

            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            createParams.CompanyId = companyId;

            // 5. create
            var jsonParams = JsonConvert.SerializeObject(createParams);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostVehicleAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<VehicleCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetVehicleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardVehicleAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetVehicleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 8. delete FKs
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            // 9. drop all
            //assetServer.

        }

        [Theory]
        [ClassData(typeof(CreateVehicleTestData))]
        public async Task delete_vehicle_response_ok_status_code(
            CreateVehicleResourceParameters createParams,
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

            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            createParams.CompanyId = companyId;

            // 5. create
            var jsonParams = JsonConvert.SerializeObject(createParams);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostVehicleAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<VehicleCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetVehicleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardVehicleAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetVehicleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 8. delete FKs
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            // 9. drop all
            //assetServer.
        }

        [Theory]
        [ClassData(typeof(UpdateVehicleTestData))]
        public async Task put_vehicle_response_ok_status_code(
            CreateVehicleResourceParameters createParams,
            UpdateVehicleResourceParameters updateParams,
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
            var postResponse = await client.PostAsync(Post.PostVehicleAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<VehicleCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetVehicleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<VehicleUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.Image.Should().Be(createParams.Image);
            newModel.Model.Description.Should().Be(createParams.Description);
            newModel.Model.NumPlate.Should().Be(createParams.NumPlate);
            newModel.Model.Brand.Should().Be(createParams.Brand);
            newModel.Model.RegisteredDate.ToString("G").Should().Be(createParams.RegisteredDate.ToString("G"));
            newModel.Model.Type.Should().Be(createParams.Type);
            newModel.Model.Status.Should().Be(createParams.Status);
            newModel.Model.Gas.Should().Be(createParams.Gas);
            newModel.Model.Height.Should().Be(createParams.Height);
            newModel.Model.Width.Should().Be(createParams.Width);
            newModel.Model.Axels.Should().Be(createParams.Axels);
            newModel.Model.MinTurnRadius.Should().Be(createParams.MinTurnRadius);
            newModel.Model.Length.Should().Be(createParams.Length);

            // 7. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateVehicleAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<VehicleModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetVehicleByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<VehicleUiModel>>(modifiedContent);

            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);
            modifiedModel.Model.Image.Should().Be(updateParams.Image);
            modifiedModel.Model.Description.Should().Be(updateParams.Description);
            modifiedModel.Model.NumPlate.Should().Be(updateParams.NumPlate);
            modifiedModel.Model.Brand.Should().Be(updateParams.Brand);
            modifiedModel.Model.RegisteredDate.ToString("G").Should().Be(updateParams.RegisteredDate.ToString("G"));
            modifiedModel.Model.Type.Should().Be(updateParams.Type);
            modifiedModel.Model.Status.Should().Be(updateParams.Status);
            modifiedModel.Model.Gas.Should().Be(updateParams.Gas);
            modifiedModel.Model.Height.Should().Be(updateParams.Height);
            modifiedModel.Model.Width.Should().Be(updateParams.Width);
            modifiedModel.Model.Axels.Should().Be(updateParams.Axels);
            modifiedModel.Model.MinTurnRadius.Should().Be(updateParams.MinTurnRadius);
            modifiedModel.Model.Length.Should().Be(updateParams.Length);


            // 8. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardVehicleAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetVehicleByIdAsync(uiModel.Model.Id));
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
