using System;

namespace sw.interprocess.api.Helpers.Exceptions.Commands
{
    public class BinaryUltrasonicPackageDetectedInboundCommandBuildException : Exception
    {
        public string ErrorMessage { get; }

        public BinaryUltrasonicPackageDetectedInboundCommandBuildException(string errorMessage)
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