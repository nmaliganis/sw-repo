using sw.interprocess.api.Helpers.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace sw.interprocess.api.Commanding.PackageCheckers
{
    public sealed class PackageChecker : IPackageChecker
    {
        private byte[] _package;

        private PackageChecker()
        {
        }

        public static IPackageChecker Checker { get; } = new PackageChecker();

        public void Check(byte[] package)
        {
            this._package = package;
            CheckSumOfMessage();
        }

        private void CheckSumOfMessage()
        {
            int agg = 0;
            var check = _package[^2..];
            string sumStringHex = System.Text.Encoding.ASCII.GetString(check);
            var checksum = Convert.ToInt32(sumStringHex, 16);
            var packageStrip = _package.Take(_package.Count() - 2).ToArray();
            foreach (byte c in packageStrip)
            {
                agg += c;
            }
            agg = agg & 0x00FF;
            if (agg != checksum)
            {
                throw new InvalidPackageChecksumException(sumStringHex);
            }
        }

        public bool IsValidPackage(string packageMessage)
        {
            var headerPackage = Regex.Matches(packageMessage, "IoT3,([^IoT]*)");
            var packages = Regex.Matches(packageMessage, "IoT1,([^IoT]*)");

            return headerPackage.Count >= 1;
        }
    }// Class: PackageChecker

}// Namespace: sw.interprocess.api.Commanding.PackageCheckers