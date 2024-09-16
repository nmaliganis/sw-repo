using System;

namespace sw.interprocess.api.Helpers.Exceptions.Commands
{
    public class GpsPackageDetectedInboundCommandBuildException : Exception
    {
        public string ErrorMessage { get; }

        public GpsPackageDetectedInboundCommandBuildException(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return BuildMessage();
        }

        public override string Message => BuildMessage();

        private string BuildMessage()
        {
            return $"Invalid Package : {ErrorMessage:X} ";
        }
    }
}
