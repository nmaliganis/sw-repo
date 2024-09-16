using System;

namespace sw.interprocess.api.Helpers.Exceptions
{
    internal class InvalidPackageExtractionException : Exception
    {
        public InvalidPackageExtractionException()
        {
        }

        public override string ToString()
        {
            return BuildMessage();
        }

        public override string Message => BuildMessage();

        private string BuildMessage()
        {
            return $"Invalid Package Extraction";
        }
    }
}