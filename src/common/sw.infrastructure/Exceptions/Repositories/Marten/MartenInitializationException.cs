using System;

namespace sw.infrastructure.Exceptions.Repositories.Marten
{
    public class MartenInitializationException : Exception
    {
        public string Details { get; }
        public string InnerExceptionDetails { get; set; }

        public MartenInitializationException(string details)
        {
            Details = details;
        }

        public MartenInitializationException(string details, string innerExceptionDetails)
        {
            Details = details;
            InnerExceptionDetails = innerExceptionDetails;
        }

        public override string Message => "Marten initialization failed.\nDetails:" + Details + InnerExceptionDetails;
    }
}
