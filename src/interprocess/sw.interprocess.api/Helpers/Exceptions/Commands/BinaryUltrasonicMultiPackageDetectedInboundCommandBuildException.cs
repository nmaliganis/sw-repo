using System;

namespace sw.interprocess.api.Helpers.Exceptions.Commands
{
    public class BinaryUltrasonicMultiPackageDetectedInboundCommandBuildException : Exception
    {
        public string ErrorMessage { get; }

        public BinaryUltrasonicMultiPackageDetectedInboundCommandBuildException(string errorMessage)
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
