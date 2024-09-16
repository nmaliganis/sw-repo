using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using dottrack.asset.common.dtos.Vms.DeviceModels;
using dottrack.asset.test.Integration.tests.Base;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dottrack.asset.test.Integration.tests.V1.DeviceModel
{
    [Collection("Scenarios")]
    public class DeviceModelScenarios
      : DeviceModelScenariosBase, IDisposable
    {

        [Theory]
        [ClassData(typeof(CreateDeviceModelTestData))]
        public async Task post_new_device_model_response_ok_status_code(CreateDeviceModelResourceParameters parameters)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();

            //Assert.NotNull(registeredasset);

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create
            var jsonParams = JsonConvert.SerializeObject(parameters);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. delete previous created id
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
        public async Task delete_device_model_response_ok_status_code(CreateDeviceModelResourceParameters parameters)
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
            var postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModelCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceModelAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [ClassData(typeof(UpdateDeviceModelTestData))]
        public async Task put_device_model_response_ok_status_code(
            CreateDeviceModelResourceParameters createParams, UpdateDeviceModelResourceParameters updateParams)
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
            var postResponse = await client.PostAsync(Post.PostDeviceModelAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

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
            newModel.Model.Enabled.Should().Be(createParams.Enabled);

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
            modifiedModel.Model.Enabled.Should().Be(updateParams.Enabled);


            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceModelAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceModelByIdAsync(uiModel.Model.Id));
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
