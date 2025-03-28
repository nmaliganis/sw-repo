﻿using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using dottrack.asset.common.dtos.ResourceParameters.Devices;
using dottrack.asset.common.dtos.Vms.Devices;
using dottrack.asset.Integration.tests.V1.Device;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;

namespace dottrack.asset.Integration.tests.V1.Sensor.ForeignKeySetups {
    internal class DeviceSetup : DeviceScenariosBase {
        public static async Task<long> GetNewDeviceId(HttpClient client, CreateDeviceResourceParameters parameters) {
            var jsonParams = JsonConvert.SerializeObject(parameters);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostDeviceAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<DeviceCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            return uiModel.Model.Id;
        }

        public static async Task DeleteDevice(HttpClient client, long id) {
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardDeviceAsync(id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetDeviceByIdAsync(id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
