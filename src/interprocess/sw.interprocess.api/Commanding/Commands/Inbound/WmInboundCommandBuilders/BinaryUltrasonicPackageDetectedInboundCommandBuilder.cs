using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders.Base;
using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands;
using sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommands.Base;
using sw.interprocess.api.Commanding.PackageRepository;
using Microsoft.Azure.Amqp.Framing;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using sw.azure.messaging.Models.IoT;
using sw.interprocess.api.Helpers.Exceptions.Commands;

namespace sw.interprocess.api.Commanding.Commands.Inbound.WmInboundCommandBuilders
{
    public class BinaryUltrasonicPackageDetectedInboundCommandBuilder : WmInboundCommandBuilder, IWmInboundCommandBuilder
    {

        public WmInboundCommand Build(string imei, string header, List<string> commandAttributes)
        {
            List<IoTUltrasonic> ultraSonicData = new List<IoTUltrasonic>();
            try
            {
                DateTime recorded = DateTime.TryParseExact($"{commandAttributes[2]} {commandAttributes[3]}",
                    "ddMMyy HHmmss", CultureInfo.CreateSpecificCulture("gr-US"),
                    DateTimeStyles.AssumeLocal, out var dt) ? dt : DateTime.UtcNow;

                var packet = commandAttributes[4];
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

                byte[] bytes = Encoding.GetEncoding(1252).GetBytes(packet);

                if (bytes.Length < 2)
                    throw new BinaryUltrasonicPackageDetectedInboundCommandBuildException("bytes.Length");

                byte headerByte = bytes[0];
                int nSamples = headerByte & 0x0F;
                int intervalMinutes = 10 * (headerByte >> 4);
                int byteLength = 4 * nSamples + 1;

                bytes = bytes.ToList().Take(byteLength).Skip(1).ToArray();

                recorded -= TimeSpan.FromMinutes((nSamples - 1) * intervalMinutes);

                for (int i = 0; i < bytes.Length; i += 4)
                {
                    ushort val = BitConverter.ToUInt16(bytes.Skip(i).Take(2).ToArray(), 0);
                    float temperature = 175.0f * ((float)val / 65535) - 45.0f;

                    byte[] b = bytes.Skip(i + 2).Take(2).ToArray();
                    int status = (b[0] >> 4) & 0x000F;
                    int range = ((b[0] << 8) + b[1]) & 0x0FFF;
                    ultraSonicData.Add(new IoTUltrasonic(imei, recorded, Convert.ToDouble(range), Convert.ToDouble(status), Convert.ToDouble(temperature)));
                    recorded += TimeSpan.FromMinutes(intervalMinutes);
                }
            }
            catch(Exception e)
            {
                ultraSonicData.Add(new IoTUltrasonic(imei, DateTime.UtcNow, 0, 0, 0));
                Log.Error(
                    $"Parsing Binary $16 packet failed: {e.Message}");
                throw new BinaryUltrasonicPackageDetectedInboundCommandBuildException(e.Message);
            }

            return new UltrasonicDetected(ultraSonicData, imei);
        }

        public override void BuildPayload()
        {
            //Todo: Refactoring to support abstract builder for Attribute JSON
        }
    }
}