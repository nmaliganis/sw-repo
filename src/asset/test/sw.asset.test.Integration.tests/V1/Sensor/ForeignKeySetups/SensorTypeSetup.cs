using dottrack.asset.common.dtos.ResourceParameters.SensorTypes;
using dottrack.asset.common.dtos.Vms.SensorTypes;
using dottrack.asset.Integration.tests.V1.SensorType;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace dottrack.asset.Integration.tests.V1.Sensor.ForeignKeySetups {
    internal class SensorTypeSetup : SensorTypeScenariosBase {
        public static async Task<long> GetNewSensorTypeId(HttpClient client, CreateSensorTypeResourceParameters parameters) {
            var jsonParams = JsonConvert.SerializeObject(parameters);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostSensorTypeAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<SensorTypeCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            return uiModel.Model.Id;
        }

        public static async Task DeleteSensorType(HttpClient client, long id) {
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardSensorTypeAsync(id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetSensorTypeByIdAsync(id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
