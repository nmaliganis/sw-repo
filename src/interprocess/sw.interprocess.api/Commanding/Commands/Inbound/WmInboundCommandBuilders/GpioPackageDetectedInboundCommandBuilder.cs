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
using System.Linq;
using System.Text;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders
{
    public class GpioPackageDetectedInboundCommandBuilder : WmInboundCommandBuilder, IWmInboundCommandBuilder
    {
        public WmInboundCommand Build(string imei, string header, List<string> commandAttributes)
        {
            AnsiConsole.Markup($"[bold white on orange3] GpioPackage[/]");
            AnsiConsole.WriteLine();
            List<IoTDigitalEvent> events = new List<IoTDigitalEvent>();

            try
            {
                DateTime recorded = DateTime.TryParseExact($"{commandAttributes[1]} {commandAttributes[2]}",
                    "ddMMyy HHmmss", CultureInfo.CreateSpecificCulture("gr-US"),
                    DateTimeStyles.AssumeLocal, out var dt) ? dt : DateTime.UtcNow;

                byte digByte = Encoding.ASCII.GetBytes(commandAttributes[3]).FirstOrDefault();
                byte digChangedByte = Encoding.ASCII.GetBytes(commandAttributes[4]).FirstOrDefault();
                bool dInput1Changed = (digChangedByte & (1 << 0)) != 0;
                bool dInput2Changed = (digChangedByte & (1 << 1)) != 0;
                bool dInput3Changed = (digChangedByte & (1 << 2)) != 0;
                bool dInput4Changed = (digChangedByte & (1 << 3)) != 0;
                bool dInput5Changed = (digChangedByte & (1 << 4)) != 0;
                bool dInput6Changed = (digChangedByte & (1 << 5)) != 0;
                bool dInput7Changed = (digChangedByte & (1 << 6)) != 0;
                bool dInput8Changed = (digChangedByte & (1 << 7)) != 0;
                byte chargerByte = Encoding.ASCII.GetBytes(commandAttributes[5]).FirstOrDefault();
                byte chargerChangedByte = Encoding.ASCII.GetBytes(commandAttributes[6]).FirstOrDefault();
                bool dInput9Changed = (chargerChangedByte & (1 << 0)) != 0;
                bool dInput10Changed = (chargerChangedByte & (1 << 1)) != 0;
                bool dInput11Changed = (chargerChangedByte & (1 << 2)) != 0;
                bool dInput12Changed = (chargerChangedByte & (1 << 3)) != 0;
                bool dInput13Changed = (chargerChangedByte & (1 << 4)) != 0;
                bool dInput14Changed = (chargerChangedByte & (1 << 5)) != 0;
                bool dInput15Changed = (chargerChangedByte & (1 << 6)) != 0;
                bool dInput16Changed = (chargerChangedByte & (1 << 7)) != 0;


                if (dInput1Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 1, (digByte & (1 << 0)) != 0 ? 1 : 0));
                if (dInput2Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 2, (digByte & (1 << 1)) != 0 ? 1 : 0));
                if (dInput3Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 3, (digByte & (1 << 2)) != 0 ? 1 : 0));
                if (dInput4Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 4, (digByte & (1 << 3)) != 0 ? 1 : 0));
                if (dInput5Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 5, (digByte & (1 << 4)) != 0 ? 1 : 0));
                if (dInput6Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 6, (digByte & (1 << 5)) != 0 ? 1 : 0));
                if (dInput7Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 7, (digByte & (1 << 6)) != 0 ? 1 : 0));
                if (dInput8Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 8, (digByte & (1 << 7)) != 0 ? 1 : 0));
                if (dInput9Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 9, (chargerByte & (1 << 0)) != 0 ? 1 : 0));
                if (dInput10Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 10, (chargerByte & (1 << 1)) != 0 ? 1 : 0));
                if (dInput11Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 11, (chargerByte & (1 << 2)) != 0 ? 1 : 0));
                if (dInput12Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 12, (chargerByte & (1 << 3)) != 0 ? 1 : 0));
                if (dInput13Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 13, (chargerByte & (1 << 4)) != 0 ? 1 : 0));
                if (dInput14Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 14, (chargerByte & (1 << 5)) != 0 ? 1 : 0));
                if (dInput15Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 15, (chargerByte & (1 << 6)) != 0 ? 1 : 0));
                if (dInput16Changed)
                    events.Add(new IoTDigitalEvent(imei, recorded, 16, (chargerByte & (1 << 7)) != 0 ? 1 : 0));

            }
            catch (Exception e)
            {
                events.Add(new IoTDigitalEvent(imei, DateTime.UtcNow, 0, 0));
                Log.Error(
                    $"Parsing Digital Event pin value change $5 packet failed: {e.Message}");
                throw new GpioPackageDetectedInboundCommandBuildException(e.Message);
            }

            return new DigitalDetected(events, imei);
        }

        public override void BuildPayload()
        {
            //Todo: Refactoring to support abstract builder for Attribute JSON
        }
    }
}