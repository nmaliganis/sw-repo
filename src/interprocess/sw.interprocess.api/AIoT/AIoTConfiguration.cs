using sw.interprocess.api.Helpers.Infrastructure;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using Serilog;
using System;
using System.Threading.Tasks;

namespace sw.interprocess.api.AIoT
{
    public class AIoTConfiguration : IAIoTConfiguration
    {
        private readonly IConfigAIoT _configAIoT;

        public AIoTConfiguration(IConfigAIoT configAIoT)
        {
            this._configAIoT = configAIoT;
        }

        public async Task ProvisionDevice(string imei)
        {
            using var security = new SecurityProviderSymmetricKey(
              //_configAIoT.RegistrationId,
              imei,
              _configAIoT.PrimaryKey,
              null);

            using ProvisioningTransportHandler transportHandler = GetTransportHandler();

            var provClient = ProvisioningDeviceClient.Create(
              _configAIoT.GlobalDeviceEndpoint,
              _configAIoT.IdScope,
              security,
              transportHandler);

            Log.Information($"Initialized for registration Id {security.GetRegistrationID()}.");

            Log.Information("Registering with the device provisioning service...");
            DeviceRegistrationResult result = await provClient.RegisterAsync();


            Log.Information($"Registration status: {result.Status}.");
            if (result.Status != ProvisioningRegistrationStatusType.Assigned)
            {
                Log.Information($"Registration status did not assign a hub, so exiting this sample.");
                return;
            }

            Log.Information($"Device {result.DeviceId} registered to {result.AssignedHub}.");

            Log.Information("Creating symmetric key authentication for IoT Hub...");
            IAuthenticationMethod auth = new DeviceAuthenticationWithRegistrySymmetricKey(
              result.DeviceId,
              security.GetPrimaryKey());

            //Todo : Needs to add the Device
            Log.Information($"Testing the provisioned device with IoT Hub...");
            //using var iotClient = DeviceClient.Create(result.AssignedHub, auth, _parameters.TransportType);
        }

        private ProvisioningTransportHandler GetTransportHandler()
        {
            return _configAIoT.TransportType switch
            {
                TransportType.Mqtt => new ProvisioningTransportHandlerMqtt(),
                TransportType.Mqtt_Tcp_Only => new ProvisioningTransportHandlerMqtt(TransportFallbackType.TcpOnly),
                TransportType.Mqtt_WebSocket_Only => new ProvisioningTransportHandlerMqtt(TransportFallbackType.WebSocketOnly),
                TransportType.Amqp => new ProvisioningTransportHandlerAmqp(),
                TransportType.Amqp_Tcp_Only => new ProvisioningTransportHandlerAmqp(TransportFallbackType.TcpOnly),
                TransportType.Amqp_WebSocket_Only => new ProvisioningTransportHandlerAmqp(TransportFallbackType.WebSocketOnly),
                TransportType.Http1 => new ProvisioningTransportHandlerHttp(),
                _ => throw new NotSupportedException($"Unsupported transport type {_configAIoT.TransportType}"),
            };
        }
    }
}