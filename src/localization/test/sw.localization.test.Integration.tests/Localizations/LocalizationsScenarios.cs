using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using dottrack.localization.common.dtos.Vms.LocalizationValues;
using dottrack.localization.test.Integration.tests.Base;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace dottrack.test.Integration.tests.Localizations
{
    [Collection("Scenarios")]
    public class LocalizationsScenarios
      : LocalizationsScenariosBase, IDisposable
    {

        [Theory]
        [InlineData("value")]
        public async Task post_new_localization_value_response_ok_status_code(string value)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var localizationServer = CreateServerLocalizationService();
            var client = localizationServer.CreateIdempotentClient();

            //Assert.NotNull(registeredLocalization);

            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create locale value
            var requestContent = new StringContent($"value={value}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var postResponse = await client.PostAsync(Post.PostLocalization(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<LocalizationValueCreationUiModel>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetLocalizationValueById(uiModel.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardLocalization(uiModel.Id));
            deleteResponse.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetLocalizationValueById(uiModel.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // 7. drop all
            //localizationServer.

        }

        [Theory]
        [InlineData("value")]
        public async Task delete_localization_value_response_ok_status_code(string value)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var localizationServer = CreateServerLocalizationService();
            var client = localizationServer.CreateIdempotentClient();


            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create locale value
            var requestContent = new StringContent($"value={value}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var postResponse = await client.PostAsync(Post.PostLocalization(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<LocalizationValueCreationUiModel>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetLocalizationValueById(uiModel.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // 6. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardLocalization(uiModel.Id));
            deleteResponse.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetLocalizationValueById(uiModel.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // 7. drop all
            //localizationServer.
        }

        [Theory]
        [InlineData("oldValue", "newValue")]
        public async Task put_localization_value_response_ok_status_code(string oldValue, string newValue)
        {
            Startup();
            using var authServer = CreateServerAuthenticactionService();
            using var localizationServer = CreateServerLocalizationService();
            var client = localizationServer.CreateIdempotentClient();


            // 1. register user
            // 2. activate user
            // 3. login user - jwt creation

            // 4. create locale oldValue
            var requestContent = new StringContent($"value={oldValue}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var postResponse = await client.PostAsync(Post.PostLocalization(), requestContent);
            postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // 5. read one - get by id, previous created id
            var responseContent = await postResponse.Content.ReadAsStringAsync();
            var uiModel = JsonConvert.DeserializeObject<LocalizationValueCreationUiModel>(responseContent);
            uiModel.Should().NotBeNull();
            uiModel.Id.Should().BePositive();

            var getResponse = await client.GetAsync(Get.GetLocalizationValueById(uiModel.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);


            // 6. update newValue
            var updateContent = new StringContent($"value={newValue}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var updateResponse = await client.PutAsync(Put.PutLocalization(uiModel.Id), updateContent);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedContent = await updateResponse.Content.ReadAsStringAsync();
            var modifiedModel = JsonConvert.DeserializeObject<LocalizationValueModificationUiModel>(updatedContent);
            modifiedModel.Should().NotBeNull();
            modifiedModel.Id.Should().BePositive();
            modifiedModel.OldValue.Should().Be(oldValue);
            modifiedModel.NewValue.Should().Be(newValue);

            // 7. delete previous created id
            var deleteResponse = await client.DeleteAsync(Delete.DeleteHardLocalization(uiModel.Id));
            deleteResponse.Should().Be(HttpStatusCode.OK);

            // ~ fetch by id, to assert null
            getResponse = await client.GetAsync(Get.GetLocalizationValueById(uiModel.Id));
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // 7. drop all
            //localizationServer.
        }

        public void Dispose()
        {
            Mapper.Reset();
        }
    }
}
