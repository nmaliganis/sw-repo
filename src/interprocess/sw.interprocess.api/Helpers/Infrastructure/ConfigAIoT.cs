using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Devices.Client;

namespace sw.interprocess.api.Helpers.Infrastructure {
    public class ConfigAIoT : IConfigAIoT {
        private readonly IConfiguration _config;

        public ConfigAIoT(IConfiguration config) {
            _config = config;
        }

        public string IdScope => _config["AIOT:IdScope"];
        public string RegistrationId => _config["AIOT:RegistrationId"];
        public string PrimaryKey => _config["AIOT:PrimaryKey"];
        public string GlobalDeviceEndpoint => _config["AIOT:GlobalDeviceEndpoint"];
        public TransportType TransportType => TransportType.Mqtt;

    }//Class : ConfigAIoT
}