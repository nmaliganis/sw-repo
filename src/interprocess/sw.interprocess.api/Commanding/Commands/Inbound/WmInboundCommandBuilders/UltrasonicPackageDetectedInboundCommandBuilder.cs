using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders.Base;
using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands;
using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using sw.azure.messaging.Models.IoT;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders
{
    public class UltrasonicPackageDetectedInboundCommandBuilder : WmInboundCommandBuilder, IWmInboundCommandBuilder
    {
        public WmInboundCommand Build(string imei, string header, List<string> commandAttributes)
        {
            List<IoTUltrasonic> payload = new List<IoTUltrasonic>();
            try
            {
                DateTime recorded = DateTime.TryParseExact($"{commandAttributes[1]} {commandAttributes[2]}",
                    "ddMMyy HHmmss", CultureInfo.CreateSpecificCulture("gr-US"),
                    DateTimeStyles.AssumeLocal, out var dt) ? dt : DateTime.UtcNow;
                double range = double.TryParse(commandAttributes[3], NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out range) ? range : 0.0;
                double status = double.TryParse(commandAttributes[4], NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out status) ? status : 0.0;
                double temperature = double.TryParse(commandAttributes[5], NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out temperature) ? temperature : 0.0;

                IoTUltrasonic ioTUltrasonic = new IoTUltrasonic(imei, recorded, range, status, temperature);
                payload.Add(ioTUltrasonic);
            }
            catch (Exception e)
            {
                payload.Add(new IoTUltrasonic(imei, DateTime.UtcNow, 0, 0, 0));
                Log.Error(
                  $"Parsing Ultrasonic $15 packet failed: {e.Message}");
            }


            return new UltrasonicDetected(payload, imei);
        }

        public override void BuildPayload()
        {
            //Todo: Refactoring to support abstract builder for Attribute JSON
        }
    }
}