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
    public class BinaryUltrasonicMultiPackageDetectedInboundCommandBuilder : WmInboundCommandBuilder, IWmInboundCommandBuilder
    {
        private string _header;
        private List<string> _commandAttributes;

        public WmInboundCommand Build(string imei, string header, List<string> commandAttributes)
        {
            AnsiConsole.Markup($"[bold white on green] BinaryUltrasonicMultiPackage[/]");
            AnsiConsole.WriteLine();

            _header = header;
            _commandAttributes = commandAttributes;
            List<IoTUltrasonic> ultraSonicData = new List<IoTUltrasonic>();
            try
            {
                DateTime recorded = DateTime.TryParseExact($"{commandAttributes[1]} {commandAttributes[2]}",
                  "ddMMyy HHmmss", CultureInfo.CreateSpecificCulture("gr-US"),
                  DateTimeStyles.AssumeLocal, out var dt) ? dt : DateTime.UtcNow;

                var packet = commandAttributes[3];
                if(packet == "f")
                    packet = commandAttributes[4];

                try
                {
                    if (int.TryParse(commandAttributes.Last(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var checksum))
                    {
                        if (commandAttributes.Count > 6)
                        {
                            var checkPacket = commandAttributes.Skip(4).ToArray();
                            packet = string.Join(",", checkPacket.Take(checkPacket.Length - 1));
                        }
                    }
                    else
                    {
                        if (commandAttributes.Count > 5)
                        {
                            var checkPacket = commandAttributes.Skip(4).ToArray();
                            packet = string.Join(",", checkPacket.Take(checkPacket.Length - 1));
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(
                        $"Parsing Multi Binary $17 packet failed: {e.Message}");
                }

                byte[] bytes = Encoding.GetEncoding(1252).GetBytes(packet);

                if (bytes.Length < 2)
                    throw new BinaryUltrasonicMultiPackageDetectedInboundCommandBuildException("bytes.Length");
                byte headerByte = bytes[0];
                int nSamples = headerByte & 0x0F;
                int intervalMinutes = 10 * (headerByte >> 4);
                int byteLength = 10 * nSamples + 1;
                packet = packet.Length > byteLength ? packet.Substring(byteLength + 1) : string.Empty;
                var parts = !string.IsNullOrWhiteSpace(packet) ? packet.Split(',').ToList() : new List<string>();
                bytes = bytes.ToList().Take(byteLength).Skip(1).ToArray();
                recorded -= TimeSpan.FromMinutes((nSamples - 1) * intervalMinutes);
                try
                {
                    for (int i = 0; i < bytes.Length; i += 10)
                    {
                        List<IoTUltrasonic> localUltraSonicData = new List<IoTUltrasonic>();
                        if (i + 2 > bytes.Length)
                        {
                            continue;
                        }
                        ushort val = BitConverter.ToUInt16(bytes.Skip(i).Take(2).ToArray(), 0);
                        float temperature = 175.0f * ((float)val / 65535) - 45.0f;
                        localUltraSonicData.Add(new IoTUltrasonic(imei, recorded, 0, 0, Convert.ToDouble(temperature)));
                        int start = i + 2;
                        int end = start + 8;
                        int x = 0;
                        try
                        {
                            for (int j = start; j < end; j += 2)
                            {
                                byte[] b = bytes.Skip(j).Take(2).ToArray();
                                if (b.Length == 2)
                                {
                                    int status = (b[0] >> 4) & 0x000F;
                                    int range = ((b[0] << 8) + b[1]) & 0x0FFF;
                                    localUltraSonicData.Add(new IoTUltrasonic(imei, recorded, Convert.ToDouble(range), Convert.ToDouble(status), 0));
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error(
                                $"Parsing Multi Binary $17 packet failed: {e.Message}");
                            throw new BinaryUltrasonicMultiPackageDetectedInboundCommandBuildException(e.Message);
                        }
                        List<double> ranges = localUltraSonicData.Skip(1).Where(u => u.Range.HasValue).Select(u => u.Range.Value).ToList();
                        List<double> statuses = localUltraSonicData.Skip(1).Where(u => u.Status.HasValue).Select(u => u.Status.Value).ToList();
                        try
                        {
                            if (ranges.Count > 0 && statuses.Count > 0)
                                localUltraSonicData.Add(new IoTUltrasonic(imei, recorded,
                                    Convert.ToInt32(ranges.Average()), Convert.ToInt32(statuses.Average()), 0));
                            recorded += TimeSpan.FromMinutes(intervalMinutes);
                            ultraSonicData.AddRange(localUltraSonicData);
                        }
                        catch (Exception e)
                        {
                            Log.Error(
                                $"Parsing Multi Binary $17 packet failed: {e.Message}");
                            throw new BinaryUltrasonicMultiPackageDetectedInboundCommandBuildException(e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(
                        $"Parsing Multi Binary $17 packet failed: {e.Message}");
                    throw new BinaryUltrasonicMultiPackageDetectedInboundCommandBuildException(e.Message);
                }
            }
            catch (Exception e)
            {
                ultraSonicData.Add(new IoTUltrasonic(imei, DateTime.UtcNow, 0, 0, 0));
                Log.Error(
                    $"Parsing Multi Binary $17 packet failed: {e.Message}");
                throw new BinaryUltrasonicMultiPackageDetectedInboundCommandBuildException(e.Message);
            }

            return new UltrasonicDetected(ultraSonicData, imei);
        }

        public override void BuildPayload()
        {
            //Todo: Refactoring to support abstract builder for Attribute JSON
        }
    }
}