using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.ResourceParameters.Assets.Containers;
using dottrack.asset.common.dtos.ResourceParameters.Companies;
using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using dottrack.asset.common.dtos.ResourceParameters.Devices;
using dottrack.asset.common.dtos.ResourceParameters.Sensor;
using dottrack.asset.common.dtos.ResourceParameters.Sensors;
using dottrack.asset.common.dtos.ResourceParameters.SensorTypes;
using dottrack.asset.common.dtos.Vms.Sensors;
using dottrack.asset.test.Integration.tests.Base;
using dottrack.asset.test.Integration.tests.V1.Sensor.ForeignKeySetups;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dottrack.asset.test.Integration.tests.V1.Sensor
{
    [Collection("Scenarios")]
    public class SensorScenarios
      : SensorScenariosBase, IDisposable
    {

        [Theory]
        [ClassData(typeof(CreateSensorTestData))]
        public async Task post_new_sensor_response_ok_status_code(
            CreateSensorResourceParameters createParams,
            CreateAssetCategoryResourceParameters assetCatParams,
            CreateCompanyResourceParameters companyParams,
            CreateContainerResourceParameters containerParams,
            CreateDeviceModelResourceParameters deviceModelParams,
            CreateDeviceResourceParameters deviceParams,
            CreateSensorTypeResourceParameters sensorTypeParams)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();

            //Assert.NotNull(registeredasset);

            // 4. create FKs
            var acId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.AssetCategoryId = acId;
            containerParams.CompanyId = companyId;
            var containerId = await ContainerSetup.GetNewContainerId(client, containerParams);

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = deviceModelId;
            var deviceId = await DeviceSetup.GetNewDeviceId(client, deviceParams);

            var senTypeId = await SensorTypeSetup.GetNewSensorTypeId(client, sensorTypeParams);

            createParams.AssetId = containerId;
            createParams.DeviceId = deviceId;
            createParams.SensorTypeId = senTypeId;

            // 5. create
            var jsonParams = JsonConvert.SerializeObject(createParams);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 8. delete FKs
            await ContainerSetup.DeleteContainer(client, containerId);
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            await DeviceSetup.DeleteDevice(client, deviceId);
            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            await SensorTypeSetup.DeleteSensorType(client, senTypeId);

            // 9. drop all
            //assetServer.

        }

        [Theory]
        [ClassData(typeof(CreateSensorTestData))]
        public async Task delete_sensor_response_ok_status_code(
            CreateSensorResourceParameters createParams,
            CreateAssetCategoryResourceParameters assetCatParams,
            CreateCompanyResourceParameters companyParams,
            CreateContainerResourceParameters containerParams,
            CreateDeviceModelResourceParameters deviceModelParams,
            CreateDeviceResourceParameters deviceParams,
            CreateSensorTypeResourceParameters sensorTypeParams)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var assetServer = CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();

            //Assert.NotNull(registeredasset);

            // 4. create FKs
            var acId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.AssetCategoryId = acId;
            containerParams.CompanyId = companyId;
            var containerId = await ContainerSetup.GetNewContainerId(client, containerParams);

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = deviceModelId;
            var deviceId = await DeviceSetup.GetNewDeviceId(client, deviceParams);

            var senTypeId = await SensorTypeSetup.GetNewSensorTypeId(client, sensorTypeParams);

            createParams.AssetId = containerId;
            createParams.DeviceId = deviceId;
            createParams.SensorTypeId = senTypeId;

            // 5. create
            var jsonParams = JsonConvert.SerializeObject(createParams);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 8. delete FKs
            await ContainerSetup.DeleteContainer(client, containerId);
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            await DeviceSetup.DeleteDevice(client, deviceId);
            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            await SensorTypeSetup.DeleteSensorType(client, senTypeId);

            // 9. drop all
            //assetServer.

        }

        [Theory]
        [ClassData(typeof(UpdateSensorTestData))]
        public async Task put_sensor_response_ok_status_code(
            CreateSensorResourceParameters createParams,
            UpdateSensorResourceParameters updateParams,
            CreateAssetCategoryResourceParameters assetCatParams,
            CreateCompanyResourceParameters companyParams,
            CreateContainerResourceParameters containerParams,
            CreateDeviceModelResourceParameters deviceModelParams,
            CreateDeviceResourceParameters deviceParams,
            CreateSensorTypeResourceParameters sensorTypeParams)
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
            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.AssetCategoryId = acId;
            containerParams.CompanyId = companyId;
            var containerId = await ContainerSetup.GetNewContainerId(client, containerParams);

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = deviceModelId;
            var deviceId = await DeviceSetup.GetNewDeviceId(client, deviceParams);

            var senTypeId = await SensorTypeSetup.GetNewSensorTypeId(client, sensorTypeParams);

            createParams.AssetId = containerId;
            createParams.DeviceId = deviceId;
            createParams.SensorTypeId = senTypeId;

            updateParams.AssetId = containerId;
            updateParams.DeviceId = deviceId;
            updateParams.SensorTypeId = senTypeId;

            // 5. create
            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<SensorUiModel>>(newContent);

            newModel.Model.Params.Should().Be(createParams.Params);
            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.IsActive.Should().Be(createParams.IsActive);
            newModel.Model.IsVisible.Should().Be(createParams.IsVisible);
            newModel.Model.Order.Should().Be(createParams.Order);
            newModel.Model.MinValue.Should().Be(createParams.MinValue);
            newModel.Model.MaxValue.Should().Be(createParams.MaxValue);
            newModel.Model.MinNotifyValue.Should().Be(createParams.MinNotifyValue);
            newModel.Model.MaxNotifyValue.Should().Be(createParams.MaxNotifyValue);
            newModel.Model.LastValue.Should().Be(createParams.LastValue);
            newModel.Model.LastRecordedDate.ToString("G").Should().Be(createParams.LastRecordedDate.ToString("G"));
            newModel.Model.LastReceivedDate.ToString("G").Should().Be(createParams.LastReceivedDate.ToString("G"));
            newModel.Model.HighThreshold.Should().Be(createParams.HighThreshold);
            newModel.Model.LowThreshold.Should().Be(createParams.LowThreshold);
            newModel.Model.SamplingInterval.Should().Be(createParams.SamplingInterval);
            newModel.Model.ReportingInterval.Should().Be(createParams.ReportingInterval);

            // 7. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateSensorAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<SensorModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<SensorUiModel>>(modifiedContent);

            modifiedModel.Model.Params.Should().Be(updateParams.Params);
            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);
            modifiedModel.Model.IsActive.Should().Be(updateParams.IsActive);
            modifiedModel.Model.IsVisible.Should().Be(updateParams.IsVisible);
            modifiedModel.Model.Order.Should().Be(updateParams.Order);
            modifiedModel.Model.MinValue.Should().Be(updateParams.MinValue);
            modifiedModel.Model.MaxValue.Should().Be(updateParams.MaxValue);
            modifiedModel.Model.MinNotifyValue.Should().Be(updateParams.MinNotifyValue);
            modifiedModel.Model.MaxNotifyValue.Should().Be(updateParams.MaxNotifyValue);
            modifiedModel.Model.LastValue.Should().Be(updateParams.LastValue);
            modifiedModel.Model.LastRecordedDate.ToString("G").Should().Be(updateParams.LastRecordedDate.ToString("G"));
            modifiedModel.Model.LastReceivedDate.ToString("G").Should().Be(updateParams.LastReceivedDate.ToString("G"));
            modifiedModel.Model.HighThreshold.Should().Be(updateParams.HighThreshold);
            modifiedModel.Model.LowThreshold.Should().Be(updateParams.LowThreshold);
            modifiedModel.Model.SamplingInterval.Should().Be(updateParams.SamplingInterval);
            modifiedModel.Model.ReportingInterval.Should().Be(updateParams.ReportingInterval);


            // 8. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 9. delete FKs
            await ContainerSetup.DeleteContainer(client, containerId);
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            await DeviceSetup.DeleteDevice(client, deviceId);
            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            await SensorTypeSetup.DeleteSensorType(client, senTypeId);

            // 10. drop all
            //assetServer.
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
