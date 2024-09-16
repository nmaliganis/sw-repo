using AutoMapper;
using dottrack.asset.common.dtos.ResourceParameters.DeviceModels;
using dottrack.asset.common.dtos.ResourceParameters.Devices;
using dottrack.asset.common.dtos.Vms.Devices;
using dottrack.asset.Integration.tests.Base.Auth;
using dottrack.asset.test.Integration.tests.Base;
using dottrack.asset.test.Integration.tests.V1.Device.ForeignKeySetups;
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

namespace dottrack.asset.Integration.tests.V1.Device
{
    [Collection("Scenarios")]
    public class DeviceScenarios
      : DeviceScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("su", "su")]
        public async Task get_fetch_all_devices_response_ok_status_code(string username, string password)
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
            var fetchedDevices = await client.GetAsync(Get.GetDevicesAsync());
            fetchedDevices.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Device
            var fetchedDevicesResponse = await fetchedDevices.Content.ReadAsStringAsync();
            var fetchedDevicesModel = JsonConvert.DeserializeObject(fetchedDevicesResponse).ToString();
            var numDevices = JObject.Parse(fetchedDevicesModel).GetValue("Value").Count();   // get the number of devices from the response        

            fetchedDevicesModel.Should().NotBe(null);
            numDevices.Should().NotBe(0);
            // 6. drop all
            Dispose();
            //authServer.
        }
        [Theory]
        [InlineData("su", "su", 3)]
        public async Task Get_Fetch_Single_Device_ByID_shouldReurn_OK(string username, string password, long validID)
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


            var fetchedDevice = await client.GetAsync(Get.GetDeviceByIdAsync(validID));
            fetchedDevice.StatusCode.Should().Be(HttpStatusCode.OK);

            // 5. Should Have At Least One Device
            var fetchedDeviceByIDResponse = await fetchedDevice.Content.ReadAsStringAsync();
            var fetchedDeviceByIDModel = JsonConvert.DeserializeObject(fetchedDeviceByIDResponse);
            fetchedDeviceByIDModel.Should().NotBe(null);
            //Assert.

            // 6. drop all
            Dispose();
            //assetServer.

        }
        [Theory]
        [InlineData("su", "su", -1)]
        public async Task Get_Fetch_Single_Device_ByinvalidID_shouldReturn_BadRequest(string username, string password, long invalidID)
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
            //If not valid ID check for BadRequest with reason being "Device Id does not exist"
            var fetchedInvalidDevice = await client.GetAsync(Get.GetDeviceByIdAsync(invalidID));
            fetchedInvalidDevice.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var fetchedInvalidDeviceByIDResponse = await fetchedInvalidDevice.Content.ReadAsStringAsync();
            string fetchedInvalidDeviceByIDMessage = JsonConvert.DeserializeObject(fetchedInvalidDeviceByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(fetchedInvalidDeviceByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Device Id does not exist", reasonOfBadRequest);

            // 6. drop all
            Dispose();
            //assetServer.

        }

        [Fact]
        public async Task createExistingDevice_response_BadRequest_deviceExists()
        {
            this.Startup();

            var newDevice = new CreateDeviceResourceParameters
            {
                Imei = "100",
                SerialNumber = "sn_100",
                PhoneNumber = "6969696969",
                IpAddress = "192.168.1.1",
                DeviceModelId = 1
            }; //Already existing Device

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

            // 4. create the same device twice so it is sure that the device exists
            var jsonCreateParams = JsonConvert.SerializeObject(newDevice);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestCreationContent);
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceCreationUiModel>>(responseContent);


            var jsonCreateParams2 = JsonConvert.SerializeObject(newDevice);
            var requestCreationContent2 = new StringContent(jsonCreateParams2, Encoding.UTF8, "application/json");
            var postResponse2 = await client.PostAsync(Post.PostDeviceAsync(), requestCreationContent2);
            postResponse2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var postResponeMessage = JsonConvert.DeserializeObject(await postResponse2.Content.ReadAsStringAsync()).ToString();
            var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
            Assert.Equal("ERROR_DEVICE_ALREADY_EXISTS", reasonOfBadRequest);


            // 7. drop all
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateDeviceTestData))]
        public async Task create_device_response_ok_status_code(
        CreateDeviceResourceParameters createParams, CreateDeviceModelResourceParameters deviceModelParams)
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

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            createParams.DeviceModelId = deviceModelId;

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestCreationContent);
            if (postResponse.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                var postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                var reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                Random rnd = new Random();
                while (reasonOfBadRequest.Equals("ERROR_DEVICE_ALREADY_EXISTS"))
                {
                    createParams.Imei = "123456789" + rnd.Next().ToString();
                    jsonCreateParams = JsonConvert.SerializeObject(createParams);
                    requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
                    postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestCreationContent);
                    postResponeMessage = JsonConvert.DeserializeObject(await postResponse.Content.ReadAsStringAsync()).ToString();
                    reasonOfBadRequest = JObject.Parse(postResponeMessage)["Messages"].First.ToString();
                } //ensure that the device does not exist
            }


            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
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
            newModel.Model.IpAddress.Should().Be(createParams.IpAddress);


            // 7. drop all/ Clean up the device created
            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        //(Skip ="Not ready yet")
        [Theory]
        [ClassData(typeof(UpdateDeviceTestData))]
        public async Task update_device_response_ok_status_code(
            CreateDeviceResourceParameters createParams,
            UpdateDeviceResourceParameters updateParams,
            CreateDeviceModelResourceParameters deviceModelParams
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
            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            createParams.DeviceModelId = deviceModelId;

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);

            // 5. read one - get by id, previous created id
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
            newModel.Model.IpAddress.Should().Be(createParams.IpAddress);

            // 6. update newValue
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
            modifiedModel.Model.IpAddress.Should().Be(updateParams.IpAddress);



            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. drop all
            //assetServer.
        }
        [Theory]
        [ClassData(typeof(CreateDeviceTestData))]
        public async Task deleteSoft_existingDevice_response_ok_status_code(CreateDeviceResourceParameters createParams, CreateDeviceModelResourceParameters deviceModelParams)
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

            // 4. create // better approach, create a device so that it exists for sure

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            createParams.DeviceModelId = deviceModelId;

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceCreationUiModel>>(responseContent);


            // 6. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteDeviceResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });

            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftDeviceAsync(uiModel.Model.Id));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request);
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            //Clean up
            var deleteHardResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(uiModel.Model.Id));
            deleteHardResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);


        }
        [Theory]
        [ClassData(typeof(CreateDeviceTestData))]
        public async Task deleteHard_existingDevice_response_ok_status_code(CreateDeviceResourceParameters createParams,
            CreateDeviceModelResourceParameters deviceModelParams)
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

            // 4. create // better approach, create a device so that it exists for sure

            var deviceModelId = await DeviceModelSetup.GetNewDeviceModelId(client, deviceModelParams);
            createParams.DeviceModelId = deviceModelId;

            var jsonCreateParams = JsonConvert.SerializeObject(createParams);
            var requestCreationContent = new StringContent(jsonCreateParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestCreationContent);
            postResponse.StatusCode.Should().Match(p => p == HttpStatusCode.Created || p == HttpStatusCode.OK);
            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceCreationUiModel>>(responseContent);


            // 6. delete 

            await DeviceModelSetup.DeleteDeviceModel(client, deviceModelId);

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(uiModel.Model.Id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            


            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(uiModel.Model.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task update_nonExistingDevice_response_BadRequest(long invalidId)
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

            var updateParams = new UpdateDeviceResourceParameters
            {
                Imei = "2883982",
                SerialNumber = "20000",
                IpAddress = "192.168.0.1"
            };
            var jsonUpdateParams = JsonConvert.SerializeObject(updateParams);
            var updateContent = new StringContent(jsonUpdateParams, Encoding.UTF8, "application/json");

            var updateResponse = await client.PutAsync(Put.UpdateDeviceAsync(invalidId), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidDeviceByIDResponse = await updateResponse.Content.ReadAsStringAsync();
            string InvalidDeviceByIDMessage = JsonConvert.DeserializeObject(InvalidDeviceByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDeviceByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Device Id does not exist", reasonOfBadRequest);

            // 7. drop all
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteSoft_nonEexistingDevice_response_badRequest(long invalidId)
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


            var getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);


            // 7. drop all/ Clean up the device created
            // 7. delete 
            var deleteJsonParams = JsonConvert.SerializeObject(new DeleteDeviceResourceParameters()
            {
                DeletedReason = "Deleted for test reasons"
            });
            var deleteRequestContent = new StringContent(deleteJsonParams, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, Delete.DeleteSoftDeviceAsync(invalidId));
            request.Content = deleteRequestContent;
            var deleteResponse = await client.SendAsync(request); //problem with DeleteAsync and body parameters
            //deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var InvalidDeviceByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidDeviceByIDMessage = JsonConvert.DeserializeObject(InvalidDeviceByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDeviceByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Device Id does not exist", reasonOfBadRequest);

            // ~ fetch by id, to assert null
            //getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(invalidId));
            //getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //assetServer.
        }

        [Theory]
        [InlineData(-1)]
        public async Task deleteHard_nonExistingDevice_response_BadRequest(long invalidId)
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

            // 4. create // better approach, create a device so that it exists for sure

            // 5. check if the device exists

            var getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(invalidId));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // 7. delete 

            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(invalidId));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var InvalidDepartmnetByIDResponse = await deleteResponse.Content.ReadAsStringAsync();
            string InvalidDeviceByIDMessage = JsonConvert.DeserializeObject(InvalidDepartmnetByIDResponse).ToString();
            var reasonOfBadRequest = JObject.Parse(InvalidDeviceByIDMessage)["Messages"].First.ToString();
            Assert.Equal("Device Id does not exist", reasonOfBadRequest);

        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
