using dottrack.asset.common.dtos.ResourceParameters.AssetCategories;
using dottrack.asset.common.dtos.Vms.AssetCategories;
using dottrack.asset.test.Integration.tests.V1.AssetCategory;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace dottrack.asset.test.Integration.tests.V1.Vehicle.ForeignKeySetups
{
    internal class AssetCategorySetup : AssetCategoryScenariosBase
    {
        public static async Task<long> GetNewAssetCategoryId(HttpClient client, CreateAssetCategoryResourceParameters parameters)
        {
            var jsonParams = JsonConvert.SerializeObject(parameters);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostAssetCategoryAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<AssetCategoryCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            return uiModel.Model.Id;
        }

        public static async Task DeleteAssetCategory(HttpClient client, long id)
        {
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardAssetCategoryAsync(id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetAssetCategoryByIdAsync(id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
