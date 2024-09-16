using dottrack.asset.common.dtos.ResourceParameters.Companies;
using dottrack.asset.common.dtos.Vms.Companies;
using dottrack.asset.test.Integration.tests.V1.Company;
using emdot.infrastructure.BrokenRules;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace dottrack.asset.test.Integration.tests.V1.Vehicle.ForeignKeySetups
{
    internal class CompanySetup : CompanyScenariosBase
    {
        public static async Task<long> GetNewCompanyId(HttpClient client, CreateCompanyResourceParameters parameters)
        {
            var jsonParams = JsonConvert.SerializeObject(parameters);
            var requestContent = new StringContent(jsonParams, Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync(Post.PostCompanyAsync(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<BusinessResult<CompanyCreationUiModel>>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Model.Id.Should().BePositive();

            return uiModel.Model.Id;
        }

        public static async Task DeleteCompany(HttpClient client, long id)
        {
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardCompanyAsync(id));
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            var getResponse = await client.GetAsync(Get.GetCompanyByIdAsync(id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
