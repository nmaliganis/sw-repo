using System;

namespace sw.auth.common.infrastructure.Exceptions.Users
{
    public class InvalidUserException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidUserException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}