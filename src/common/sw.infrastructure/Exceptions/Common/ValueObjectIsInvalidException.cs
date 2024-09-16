using System;

namespace sw.infrastructure.Exceptions.Common
{
    public class ValueObjectIsInvalidException : Exception
    {
        public ValueObjectIsInvalidException(string message)
            : base(message)
        {

        }
    }
}