using sw.interprocess.api.Commanding.Commands.Inbound;
using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;
using sw.interprocess.api.Commanding.Listeners.Inbounds;
using sw.interprocess.api.Commanding.PackageCheckers;
using sw.interprocess.api.Commanding.PackageExtractor;
using sw.interprocess.api.Commanding.PackageRepository;
using sw.interprocess.api.Commanding.Servers;
using sw.interprocess.api.Helpers.Exceptions;
using sw.interprocess.api.Models.Messages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace sw.interprocess.api.Mqtt
{
    public class MqttConfiguration : IMqttConfiguration, ITelemetryRowDetectionActionListener
    {
        public IConfiguration Configuration { get; }


        private MqttClient _client;

        public MqttConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;

            WmInboundServer.GetWmInboundServer.Attach(this);
        }

        public void EstablishConnection()
        {
            _client = new MqttClient(Configuration.GetSection("Mqtt:Host").Value);

            _client.Subscribe(new[]
              {
                  Configuration.GetSection("MqttTopics:Telemetry").Value
        },
              new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            _client.MqttMsgPublishReceived += ClientMqttMsgPublishReceived;
            _client.ConnectionClosed += ClientConnectionClosed;
            _client.MqttMsgPublished += ClientMqttMsgPublished;
            _client.MqttMsgSubscribed += ClientMqttMsgSubscribed;
            _client.MqttMsgUnsubscribed += ClientMqttMsgUnsubscribed;

            _client.Connect($"{Configuration.GetSection("Mqtt:ClientId").Value}-{Guid.NewGuid().ToString()}",
              Configuration.GetSection("Mqtt:Username").Value
              , Configuration.GetSection("Mqtt:Password").Value
            );
        }

        private void ClientMqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
        }

        private void ClientMqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Log.Information($"MQTT Subscribed for {e.MessageId} After at :{DateTime.UtcNow}");
        }

        private void ClientMqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Log.Information($"MQTT Published. After at :{DateTime.UtcNow}");
        }

        private void ClientConnectionClosed(object sender, EventArgs e)
        {
            Log.Information($"MQTT Closed. After at :{DateTime.UtcNow}");
            Log.Fatal(e.ToString()!);
        }

        private void ClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if(e.Topic.Contains("dummy"))
                return;

            Log.Information($"MQTT Received for {e.Topic}. After at :{DateTime.UtcNow}");

            byte[] package = e.Message;

            try
            {
                string packageMessage = Encoding.GetEncoding(WmPackageRepository.GetPackageRepository.EncodingCode)
                    .GetString(e.Message);

                if (PackageChecker.Checker.IsValidPackage(packageMessage))
                {
                    //PackageChecker.Checker.Check(package);

                    WasteMessage wasteMessage = PackageExtractor.Extractor.ExtractPackages(packageMessage);


                    if (wasteMessage.Commands.Count == 0)
                        return;

                    foreach (var wasteMessageCommand in wasteMessage.Commands)
                    {
                        try
                        {
                            Thread.Sleep(100);
                            WmInboundCommandBuilderRepository.GetCommandBuilderRepository
                                    [wasteMessageCommand.Split(',').ToList().ElementAt(0)]
                                .Build(wasteMessage.Imei, wasteMessage.Header, wasteMessageCommand.Split(',').ToList())
                                .RaiseWmEvent(WmInboundServer.GetWmInboundServer);
                        }
                        catch (Exception ex)
                        {
                            AnsiConsole.WriteException(ex);
                            Log.Fatal($"Parsing Error : {ex.Message}. After at :{DateTime.UtcNow}");
                            AnsiConsole.WriteLine();
                            AnsiConsole.Markup($"[bold white on red] ERROR_PARSING_PACKAGE[/]");
                            AnsiConsole.WriteLine();
                        }
                    }
                }
            }
            catch (InvalidPackageExtractionException ex)
            {
                AnsiConsole.WriteException(ex);
                string errorMessage = "ERROR_INVALID_PACKAGE_EXTRACTION";
                Log.Error(
                  $"ClientMqtt Msg PublishReceived: {e.Message}" +
                  $"Error Message:{errorMessage}" +
                  "--ClientMqttPublishReceived--  @NotComplete@ [MqttConfiguration]. " +
                  $"Broken rules: {ex.Message}");
            }
            catch (InvalidPackageCrcException ex)
            {
                AnsiConsole.WriteException(ex);
                string errorMessage = "ERROR_INVALID_PACKAGE_CRC";
                Log.Error(
                  $"ClientMqtt Msg PublishReceived: {e.Message}" +
                  $"Error Message:{errorMessage}" +
                  "--ClientMqttPublishReceived--  @NotComplete@ [MqttConfiguration]. " +
                  $"Broken rules: {ex.Message}");
            }
            catch (InvalidPackageChecksumException ex)
            {
                AnsiConsole.WriteException(ex);
                string errorMessage = "ERROR_INVALID_PACKAGE_CHECKSUM";
                Log.Error(
                  $"ClientMqtt Msg PublishReceived: {e.Message}" +
                  $"Error Message:{errorMessage}" +
                  "--ClientMqttPublishReceived--  @NotComplete@ [MqttConfiguration]. " +
                  $"Broken rules: {ex.Message}");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                string errorMessage = "UNKNOWN_ERROR";
                Log.Error(
                  $"Create Container: {e.Message}" +
                  $"Error Message:{errorMessage}" +
                  "--ClientMqttPublishReceived--  @NotComplete@ [CreateContainerProcessor]. " +
                  $"Broken rules: {ex.Message}");
            }
        }

        public void Update(object sender, TelemetryRowDetectionEventArgs e)
        {
            if (_client.IsConnected)
            {
                var result = _client.Publish(Configuration.GetSection("MqttTopics:Telemetry").Value, Encoding.UTF8.GetBytes(e.Payload));
            }
        }
    }// Class: MqttConfiguraton
}// Namespace: sw.interprocess.api.Mqtt