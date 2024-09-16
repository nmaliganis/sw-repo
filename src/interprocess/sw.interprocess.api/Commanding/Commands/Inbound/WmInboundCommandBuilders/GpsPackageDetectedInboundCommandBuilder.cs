using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders.Base;
using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands;
using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using sw.interprocess.api.Helpers.Exceptions.Commands;
using sw.azure.messaging.Models.IoT;
using Serilog;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders
{
    public class GpsPackageDetectedInboundCommandBuilder : WmInboundCommandBuilder, IWmInboundCommandBuilder
    {

        public WmInboundCommand Build(string imei, string header, List<string> commandAttributes)
        {
            AnsiConsole.Markup($"[bold white on lime] slateblue3[/]");
            AnsiConsole.WriteLine();

            IoTgps ioTGPS;
            try
            {
                DateTime recorded = DateTime.TryParseExact(
                    $"{commandAttributes[10]} {commandAttributes[1].Split('.')[0]}",
                    "ddMMyy HHmmss", CultureInfo.CreateSpecificCulture("gr-US"),
                    DateTimeStyles.AssumeLocal, out var dt) ? dt : DateTime.UtcNow;

                double latitude = double.TryParse(commandAttributes[2].TrimEnd('N'), NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out double la) ? la * 0.01 : 0.0;
                double longitude = double.TryParse(commandAttributes[3].TrimEnd('E'), NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out double lo) ? lo * 0.01 : 0.0;
                double integerPart = Math.Floor(latitude);
                double decimalPart = 100.0 * (latitude - Math.Truncate(latitude));
                latitude = integerPart + decimalPart / 60.0;
                integerPart = Math.Floor(longitude);
                decimalPart = 100.0 * (longitude - Math.Truncate(longitude));
                longitude = integerPart + decimalPart / 60.0;
                double altitude = double.TryParse(commandAttributes[5], NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out altitude) ? altitude : 0.0;
                double speed = double.TryParse(commandAttributes[8], NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out speed) ? speed : 0.0;
                double direction = double.TryParse(commandAttributes[7], NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out direction) ? direction : 0.0;
                int fixMode = int.TryParse(commandAttributes[6], out fixMode) ? fixMode : 0;
                double hdop = double.TryParse(commandAttributes[4], NumberStyles.Number, CultureInfo.CreateSpecificCulture("gr-US"), out hdop) ? hdop : 0.0;
                int satellitesUsed = int.TryParse(commandAttributes[11], out satellitesUsed) ? satellitesUsed : 0;

                ioTGPS = new IoTgps(imei, recorded, latitude, longitude, altitude, speed, direction, fixMode, hdop, satellitesUsed);

            }
            catch (Exception e)
            {
                ioTGPS = new IoTgps(imei, DateTime.UtcNow, 0, 0, 0, 0, 0, 0, 0, 0);
                Log.Error(
                    $"Parsing GPS $21 packet failed: {e.Message}");
                throw new GpsPackageDetectedInboundCommandBuildException(e.Message);
            }

            return new GPSDetected(ioTGPS, imei);
        }

        public override void BuildPayload()
        {
            //Todo: Refactoring to support abstract builder for Attribute JSON
        }
    }
}