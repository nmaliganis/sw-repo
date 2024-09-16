using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.SensorTypes;
using dottrack.asset.common.dtos.Vms.SensorTypes;
using dottrack.asset.test.Integration.tests.Base;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dottrack.asset.test.Integration.tests.V1.SensorType
{
    [Collection("Scenarios")]
    public class SensorTypeScenarios
      : SensorTypeScenariosBase, IDisposable
    {

        [Theory]
        [ClassData(typeof(CreateSensorTypeTestData))]
        public async Task post_new_sensor_type_response_ok_status_code(CreateSensorTypeResourceParameters parameters)
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
            var postResponse = await client.PostAsync(Post.PostSensorTypeAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorTypeCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSensorTypeByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorTypeAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSensorTypeByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.

        }

        [Theory]
        [ClassData(typeof(CreateSensorTypeTestData))]
        public async Task delete_sensor_type_response_ok_status_code(CreateSensorTypeResourceParameters parameters)
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
            var postResponse = await client.PostAsync(Post.PostSensorTypeAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorTypeCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSensorTypeByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorTypeAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSensorTypeByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [ClassData(typeof(UpdateSensorTypeTestData))]
        public async Task put_sensor_type_response_ok_status_code(
            CreateSensorTypeResourceParameters createParams, UpdateSensorTypeResourceParameters updateParams)
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
            var postResponse = await client.PostAsync(Post.PostSensorTypeAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorTypeCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSensorTypeByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<SensorTypeUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);

            // 6. update newValue
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");
            var updateResponse = await client.PutAsync(Put.UpdateSensorTypeAsync(uiModel.Model.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedModel = JsonConvert.DeserializeObject<BusinessResult<SensorTypeModificationUiModel>>(updatedContent);
            updatedModel.Should().NotBeNull();
            updatedModel.Model.Id.Should().BePositive();

            getResponse = await client.GetAsync(Get.GetSensorTypeByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var modifiedContent = await getResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<BusinessResult<SensorTypeUiModel>>(modifiedContent);

            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorTypeAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSensorTypeByIdAsync(uiModel.Model.Id));
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
