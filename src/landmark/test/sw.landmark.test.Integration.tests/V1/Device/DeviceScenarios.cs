using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using dottrack.asset.common.dtos.ResourceParameters.Devices;
using dottrack.asset.common.dtos.Vms.Devices;
using dottrack.asset.test.Integration.tests.Base;
using dottrack.asset.test.Integration.tests.V1.Device.ForeignKeySetups;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dottrack.asset.test.Integration.tests.V1.Device
{
    [Collection("Scenarios")]
    public class DeviceScenarios
      : DeviceScenariosBase, IDisposable
    {

        [Theory]
        [ClassData(typeof(CreateDeviceTestData))]
        public async Task post_new_device_response_ok_status_code(
            CreateDeviceResourceParameters deviceParams,
            CreateDeviceModelResourceParameters deviceModelParams)
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
            var dmId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = dmId;

            // 5. create
            var jsonParams = JsonConvert.SerializeObject(deviceParams);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 8. delete FKs
            await DeviceModelSetup.DeleteDeviceModel(client, dmId);

            // 9. drop all
            //assetServer.

        }

        [Theory]
        [ClassData(typeof(CreateDeviceTestData))]
        public async Task delete_device_response_ok_status_code(
            CreateDeviceResourceParameters deviceParams,
            CreateDeviceModelResourceParameters deviceModelParams)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();


            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create FKs
            var dmId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = dmId;

            // 5. create
            var jsonParams = JsonConvert.SerializeObject(deviceParams);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 8. delete FKs
            await DeviceModelSetup.DeleteDeviceModel(client, dmId);

            // 9. drop all
            //assetServer.
        }

        [Theory]
        [ClassData(typeof(UpdateDeviceTestData))]
        public async Task put_device_response_ok_status_code(
            CreateDeviceResourceParameters createParams,
            UpdateDeviceResourceParameters updateParams,
            CreateDeviceModelResourceParameters deviceModelParams)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();


            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create FKs
            var dmId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            createParams.DeviceModelId = dmId;
            updateParams.DeviceModelId = dmId;

            // 5. create
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<DeviceUiModel>>(newContent);

            newModel.Model.Imei.Should().Be(createParams.Imei);
            newModel.Model.SerialNumber.Should().Be(createParams.SerialNumber);
            newModel.Model.ActivationCode.Should().Be(createParams.ActivationCode);
            newModel.Model.ActivationDate.ToString("G").Should().Be(createParams.ActivationDate.ToString("G"));
            newModel.Model.ActivationBy.Should().Be(createParams.ActivationBy);
            newModel.Model.ProvisioningCode.Should().Be(createParams.ProvisioningCode);
            newModel.Model.ProvisioningBy.Should().Be(createParams.ProvisioningBy);
            newModel.Model.ProvisioningDate.ToString("G").Should().Be(createParams.ProvisioningDate.ToString("G"));
            newModel.Model.ResetCode.Should().Be(createParams.ResetCode);
            newModel.Model.ResetBy.Should().Be(createParams.ResetBy);
            newModel.Model.ResetDate.ToString("G").Should().Be(createParams.ResetDate.ToString("G"));
            newModel.Model.Activated.Should().Be(createParams.Activated);
            newModel.Model.Enabled.Should().Be(createParams.Enabled);
            newModel.Model.IpAddress.Should().Be(createParams.IpAddress);
            newModel.Model.LastRecordedDate.ToString("G").Should().Be(createParams.LastRecordedDate.ToString("G"));
            newModel.Model.LastReceivedDate.ToString("G").Should().Be(createParams.LastReceivedDate.ToString("G"));
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);


            // 7. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateDeviceAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<DeviceModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<DeviceUiModel>>(modifiedContent);

            modifiedModel.Model.Imei.Should().Be(updateParams.Imei);
            modifiedModel.Model.SerialNumber.Should().Be(updateParams.SerialNumber);
            modifiedModel.Model.ActivationCode.Should().Be(updateParams.ActivationCode);
            modifiedModel.Model.ActivationDate.ToString("G").Should().Be(updateParams.ActivationDate.ToString("G"));
            modifiedModel.Model.ActivationBy.Should().Be(updateParams.ActivationBy);
            modifiedModel.Model.ProvisioningCode.Should().Be(updateParams.ProvisioningCode);
            modifiedModel.Model.ProvisioningBy.Should().Be(updateParams.ProvisioningBy);
            modifiedModel.Model.ProvisioningDate.ToString("G").Should().Be(updateParams.ProvisioningDate.ToString("G"));
            modifiedModel.Model.ResetCode.Should().Be(updateParams.ResetCode);
            modifiedModel.Model.ResetBy.Should().Be(updateParams.ResetBy);
            modifiedModel.Model.ResetDate.ToString("G").Should().Be(updateParams.ResetDate.ToString("G"));
            modifiedModel.Model.Activated.Should().Be(updateParams.Activated);
            modifiedModel.Model.Enabled.Should().Be(updateParams.Enabled);
            modifiedModel.Model.IpAddress.Should().Be(updateParams.IpAddress);
            modifiedModel.Model.LastRecordedDate.ToString("G").Should().Be(updateParams.LastRecordedDate.ToString("G"));
            modifiedModel.Model.LastReceivedDate.ToString("G").Should().Be(updateParams.LastReceivedDate.ToString("G"));
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);


            // 8. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 9. delete FKs
            await DeviceModelSetup.DeleteDeviceModel(client, dmId);

            // 10. drop all
            //assetServer.
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
