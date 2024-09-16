using System;

namespace sw.interprocess.api.Helpers.Exceptions
{
    internal class InvalidPackageCrcException : Exception
    {
        private readonly byte _crcValue;

        public InvalidPackageCrcException(byte crcValue)
        {
            _crcValue = crcValue;
        }

        public override string ToString()
        {
            return BuildMessage();
        }

        public override string Message => BuildMessage();

        private string BuildMessage()
        {
            return $"Invalid Package Crc culc: {_crcValue:X} ";
        }
    }
}