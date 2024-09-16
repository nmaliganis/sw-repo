using sw.interprocess.api.Helpers.Exceptions;
using sw.interprocess.api.Models.Messages;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace sw.interprocess.api.Commanding.PackageExtractor
{
    public sealed class PackageExtractor : IPackageExtractor
    {
        private PackageExtractor()
        {
        }

        public static IPackageExtractor Extractor { get; } = new PackageExtractor();

        public WasteMessage ExtractPackages(string packageMessage)
        {
            WasteMessage wasteMessage = new WasteMessage();
            var headerPackage = Regex.Matches(packageMessage, "IoT3,([^IoT]*)");
            var packages = Regex.Matches(packageMessage, "IoT1,([^IoT]*)");

            if (headerPackage.Count == 0)
            {
                throw new InvalidPackageExtractionException();
            }
            if (packages.Count == 0)
            {
                throw new InvalidPackageExtractionException();
            }

            for (int i = 0; i < headerPackage.Count; i++)
            {
                if (i == 0)
                {
                    wasteMessage.Value = packages[i].Value;
                    wasteMessage.Header = headerPackage[i].Value;
                    wasteMessage = ExtractImeiPackage(wasteMessage);
                    wasteMessage = ExtractDate(wasteMessage);
                    wasteMessage.Commands.Add(packageMessage.TrimStart(wasteMessage.Header.ToCharArray()));
                }
            }

            return wasteMessage;
        }

        private WasteMessage ExtractImeiPackage(WasteMessage wasteMessage)
        {
            wasteMessage.Imei = wasteMessage.Header.Split(',').ToList().ElementAt(1);
            return wasteMessage;
        }

        private WasteMessage ExtractDate(WasteMessage wasteMessage)
        {
            var dateTime = wasteMessage.Value.Split(',').Skip(2).Take(2).ToList(); // Skip 2, IoT1 and Imei
            wasteMessage.Date = DateTime.TryParseExact($"{dateTime[0]} {dateTime[1]}",
                "ddMMyy HHmmss", CultureInfo.CreateSpecificCulture("gr-US"),
                DateTimeStyles.AssumeLocal, out var dt) ? dt : DateTime.UtcNow;
            return wasteMessage;
        }
    }
}
