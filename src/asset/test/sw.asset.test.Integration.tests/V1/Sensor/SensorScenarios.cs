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
using dottrack.asset.Integration.tests.Base.Auth;
using dottrack.asset.Integration.tests.V1.Sensor.ForeignKeySetups;
using dottrack.asset.test.Integration.tests.Base;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Microsoft.Azure.Amqp.Framing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dottrack.asset.Integration.tests.V1.Sensor
{
    [Collection("Scenarios")]
    public class SensorScenarios
      : SensorScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_sensors_response_ok_status_code(string username, string password)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);
            var fetchedSensors = await client.GetAsync(Get.GetSensorsAsync());
            fetchedSensors.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Sensor
            var fetchedSensorsResponse = await fetchedSensors.Content.ReadAsStringAsync();
            var fetchedSensorsModel = JsonConvert.DeserializeObject(fetchedSensorsResponse).ToString();
            var numSensors = JObject.Parse(fetchedSensorsModel).GetValue("Value").Count();   // get the number of sensors from the response        

            fetchedSensorsModel.Should().NotBe(null);
            numSensors.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 3)]
        public async Task Get_Fetch_Single_Sensor_ByID_shouldReurn_OK(string username, string password, long validID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. read all - Fetch all


            var fetchedSensor = await client.GetAsync(Get.GetSensorByIdAsync(validID));
            fetchedSensor.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Sensor
            var fetchedSensorByIDResponse = await fetchedSensor.Content.ReadAsStringAsync();
            var fetchedSensorByIDModel = JsonConvert.DeserializeObject(fetchedSensorByIDResponse);
            fetchedSensorByIDModel.Should().NotBe(null);
            //Assert.

            // 6. drop all
            Dispose();
            //assetServer.

        }
        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_Sensor_ByinvalidID_shouldReturn_BadRequest(string username, string password, long invalidID)
        {
            Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            //Assert.
            //If not valid ID check for BadRequest with reason being "Sensor Id does not exist"
            var fetchedInvalidSensor = await client.GetAsync(Get.GetSensorByIdAsync(invalidID));
            fetchedInvalidSensor.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidSensorByIDResponse = await fetchedInvalidSensor.Content.ReadAsStringAsync();
            string fetchedInvalidSensorByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidSensorByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidSensorByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Sensor Id does not exist", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Fact]
        public async Task createExistingSensor_response_BadRequest_sensorExists()
        {
            this.Startup();
            var rnd = new Random();
            var newSensor = new CreateSensorResourceParameters
            {
                Params = "{}",
                Name = "Sen1" + rnd.Next(),
                CodeErp = "1000",
                IsActive = true,
                IsVisible = true,
                Order = 1,
                MinValue = 1,
                MaxValue = 1,
                MinNotifyValue = 1,
                MaxNotifyValue = 1,
                LastValue = 1,
                LastRecordedDate = DateTime.Now,
                LastReceivedDate = DateTime.Now,
                HighThreshold = 1,
                LowThreshold = 1,
                SamplingInterval = 1,
                ReportingInterval = 1,
                AssetId = 1,
                DeviceId = 3,
                SensorTypeIndex = 3
            }; //Already existing Sensor

            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create the same sensor twice so it is sure that the sensor exists
            var jsonCreateParams = JsonConvert.SerializeObject(newSensor);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorAsync(), requestCreationContent);
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorCreationUiModel>>(responseContent);


            var jsonCreateParams2 = JsonConvert.SerializeObject(newSensor);
            var requestCreationContent2 = new StringContent(jsonCreateParams2, Encoding.UTF8, "application/json");
            var postResponse2 = await client.PostAsync(Post.PostSensorAsync(), requestCreationContent2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var postResponeMessage = JsonConvert.DeserializeObject(await postResponse2.Content.ReadAsStringAsync()).ToString();
            var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
            Assert.Equal("ERROR_SENSOR_ALREADY_EXISTS", reasonOfBadRequest);


            // 7. drop all
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateSensorTestData))]
        public async Task create_sensor_response_ok_status_code(
        CreateSensorResourceParameters createParams,
        CreateAssetCategoryResourceParameters assetCatParams,
        CreateCompanyResourceParameters companyParams,
        CreateContainerResourceParameters containerParams,
        CreateDeviceModelResourceParameters deviceModelParams,
        CreateDeviceResourceParameters deviceParams,
        CreateSensorTypeResourceParameters sensorTypeParams)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create

            var acId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.AssetCategoryId = acId;
            containerParams.CompanyId = companyId;
            var containerId = await ContainerSetup.GetNewContainerId(client, containerParams);

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = deviceModelId;
            var deviceId = await DeviceSetup.GetNewDeviceId(client, deviceParams);

            var senTypeId = await SensorTypeSetup.GetNewSensorTypeId(client, sensorTypeParams);

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_DEVICE_ALREADY_EXISTS"))
                {
                    createParams.Name = "123456789" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostSensorAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the sensor does not exist
            }


            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<SensorUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.MaxValue.Should().Be(createParams.MaxValue);


            // 7. drop all/ Clean up the sensor created
            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);



            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.

            await ContainerSetup.DeleteContainer(client, containerId);
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            await DeviceSetup.DeleteDevice(client, deviceId);
            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            await SensorTypeSetup.DeleteSensorType(client, senTypeId);
        }

        //(Skip ="Not ready yet")
        [Theory]
        [ClassData(typeof(UpdateSensorTestData))]
        public async Task update_sensor_response_ok_status_code(
            CreateSensorResourceParameters createParams,
            UpdateSensorResourceParameters updateParams,
        CreateAssetCategoryResourceParameters assetCatParams,
        CreateCompanyResourceParameters companyParams,
        CreateContainerResourceParameters containerParams,
        CreateDeviceModelResourceParameters deviceModelParams,
        CreateDeviceResourceParameters deviceParams,
        CreateSensorTypeResourceParameters sensorTypeParams
        )
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create
            var acId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.AssetCategoryId = acId;
            containerParams.CompanyId = companyId;
            var containerId = await ContainerSetup.GetNewContainerId(client, containerParams);

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = deviceModelId;
            var deviceId = await DeviceSetup.GetNewDeviceId(client, deviceParams);

            var senTypeId = await SensorTypeSetup.GetNewSensorTypeId(client, sensorTypeParams);

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var newContent = await getResponse.Content.ReadAsStringAsync();
            var newModel = JsonConvert.DeserializeObject<BusinessResult<SensorUiModel>>(newContent);

            newModel.Model.Name.Should().Be(createParams.Name);
            newModel.Model.CodeErp.Should().Be(createParams.CodeErp);
            newModel.Model.Order.Should().Be(createParams.Order);

            // 6. update newValue
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

            modifiedModel.Model.Name.Should().Be(updateParams.Name);
            modifiedModel.Model.CodeErp.Should().Be(updateParams.CodeErp);
            modifiedModel.Model.MaxValue.Should().Be(updateParams.MaxValue);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);



            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all


            await ContainerSetup.DeleteContainer(client, containerId);
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            await DeviceSetup.DeleteDevice(client, deviceId);
            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            await SensorTypeSetup.DeleteSensorType(client, senTypeId);
            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateSensorTestData))]
        public async Task deleteSoft_existingSensor_response_ok_status_code(CreateSensorResourceParameters createParams,
        CreateAssetCategoryResourceParameters assetCatParams,
        CreateCompanyResourceParameters companyParams,
        CreateContainerResourceParameters containerParams,
        CreateDeviceModelResourceParameters deviceModelParams,
        CreateDeviceResourceParameters deviceParams,
        CreateSensorTypeResourceParameters sensorTypeParams)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create // better approach, create a sensor so that it exists for sure

            var acId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.AssetCategoryId = acId;
            containerParams.CompanyId = companyId;
            var containerId = await ContainerSetup.GetNewContainerId(client, containerParams);

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = deviceModelId;
            var deviceId = await DeviceSetup.GetNewDeviceId(client, deviceParams);

            var senTypeId = await SensorTypeSetup.GetNewSensorTypeId(client, sensorTypeParams);

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorCreationUiModel>>(responseContent);


            // 6. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteSensorResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });

            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftSensorAsync(uiModel.Model.Id));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //Clean up
            var deleteHardResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(uiModel.Model.Id));
            deleteHardResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            await ContainerSetup.DeleteContainer(client, containerId);
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            await DeviceSetup.DeleteDevice(client, deviceId);
            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            await SensorTypeSetup.DeleteSensorType(client, senTypeId);


        }
        [Theory]
        [ClassData(typeof(CreateSensorTestData))]
        public async Task deleteHard_existingSensor_response_ok_status_code(CreateSensorResourceParameters createParams,
        CreateAssetCategoryResourceParameters assetCatParams,
        CreateCompanyResourceParameters companyParams,
        CreateContainerResourceParameters containerParams,
        CreateDeviceModelResourceParameters deviceModelParams,
        CreateDeviceResourceParameters deviceParams,
        CreateSensorTypeResourceParameters sensorTypeParams)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create // better approach, create a sensor so that it exists for sure

            var acId = await AssetCategorySetup.GetNewAssetCategoryId(client, assetCatParams);
            var companyId = await CompanySetup.GetNewCompanyId(client, companyParams);
            containerParams.AssetCategoryId = acId;
            containerParams.CompanyId = companyId;
            var containerId = await ContainerSetup.GetNewContainerId(client, containerParams);

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            deviceParams.DeviceModelId = deviceModelId;
            var deviceId = await DeviceSetup.GetNewDeviceId(client, deviceParams);

            var senTypeId = await SensorTypeSetup.GetNewSensorTypeId(client, sensorTypeParams);

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorCreationUiModel>>(responseContent);


            // 6. delete 



            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);




            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.


            await ContainerSetup.DeleteContainer(client, containerId);
            await AssetCategorySetup.DeleteAssetCategory(client, acId);
            await CompanySetup.DeleteCompany(client, companyId);

            await DeviceSetup.DeleteDevice(client, deviceId);
            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            await SensorTypeSetup.DeleteSensorType(client, senTypeId);
        }

        [Theory]
        [InlineData(-1)]
        public async Task update_nonExistingSensor_response_BadRequest(long invalidId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create


            // 5. read one - get by id, previous created id



            // 6. update newValue

            var updateParams = new UpdateSensorResourceParameters
            {
                Name = "2883982",
                CodeErp = "20000",
                MinValue = 1000,
                LastReceivedDate = DateTime.UtcNow,
                LastRecordedDate = DateTime.UtcNow,
                Order =1,
                IsActive = true,
                IsVisible = true,
                Params = "{Test:test}",
                SensorTypeId = 3,
                DeviceId = 3,
                AssetId = 1
            };
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");

            var updateResponse = await client.PutAsync(Put.UpdateSensorAsync(invalidId), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidSensorByIDResponse = await updateResponse.Content.ReadAsStringAsync();
            string InvalidSensorByIDMessage = JsonConvert.DeserializeObject(InvalidSensorByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidSensorByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Sensor Id does not exist", reasonOfBadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingSensor_response_badRequest(long invalidId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create


            // 5. read one - get by id, previous created id


            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the sensor created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteSensorResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftSensorAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
                                                                  //deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidSensorByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidSensorByIDMessage = JsonConvert.DeserializeObject(InvalidSensorByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidSensorByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Sensor Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetSensorByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingSensor_response_BadRequest(long invalidId)
        {
            this.Startup();
            using var authServer = this.CreateServerAuthenticactionService();

            var authClient = authServer.CreateIdempotentClient();

            string username = "su";
            string password = "su";

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation
            var logJsonParams = JsonConvert.SerializeObject(new LoginModel()
            {
                Login = username,
                Password = password
            });
            var rqstContent = new StringContent(logJsonParams, Encoding.UTF8, "application/json");
            var authorizedTestUser = await authClient.PostAsync(PostJwt.PostLogin(), rqstContent);
            authorizedTestUser.StatusCode.Should().Be(HttpStatusCode.OK);

            var authorizedTestUserAuthToken = await authorizedTestUser.Content.ReadAsStringAsync();
            var authUiModel = JsonConvert.DeserializeObject<AuthModel>(authorizedTestUserAuthToken);
            authUiModel.Should().NotBe(null);
            Assert.NotEmpty(authUiModel!.Token);

            using var assetServer = this.CreateServerAssetService();
            var client = assetServer.CreateIdempotentClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authUiModel!.Token);

            // 4. create // better approach, create a sensor so that it exists for sure

            // 5. check if the sensor exists

            var getResponse = await client.GetAsync(Get.GetSensorByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidDepartmnetByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidSensorByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmnetByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidSensorByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Sensor Id does not exist", reasonOfBadRequest);

        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
