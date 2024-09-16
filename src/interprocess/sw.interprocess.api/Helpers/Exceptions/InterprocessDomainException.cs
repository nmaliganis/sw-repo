using System;

namespace sw.interprocess.api.Helpers.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class InterprocessDomainException : Exception
    {
        public InterprocessDomainException()
        {
        }

        public InterprocessDomainException(string message)
          : base(message)
        {
        }

        public InterprocessDomainException(string message, Exception innerException)
          : base(message, innerException)
        {
        }
    } //Class : AuthDomainException
}