using dottrack.asset.common.dtos.ResourceParameters.Assets.Containers;
using dottrack.asset.common.dtos.Vms.Assets.Containers;
using dottrack.asset.Integration.tests.V1.Container;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace dottrack.asset.Integration.tests.V1.Sensor.ForeignKeySetups {
    internal class ContainerSetup : ContainerScenariosBase {
        public static async Task<long> GetNewContainerId(HttpClient client, CreateContainerResourceParameters parameters) {
            var jsonParams = JsonConvert.SerializeObject(parameters);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostContainerAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<ContainerCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            return uiModel.Model.Id;
        }

        public static async Task DeleteContainer(HttpClient client, long id) {
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardContainerAsync(id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetContainerByIdAsync(id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
