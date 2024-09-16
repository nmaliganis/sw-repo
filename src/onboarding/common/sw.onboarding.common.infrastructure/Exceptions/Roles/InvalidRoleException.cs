using System;

namespace sw.auth.common.infrastructure.Exceptions.Roles
{
    public class InvalidRoleException : Exception
    {
        public string BrokenRules { get; private set; }

        public InvalidRoleException(string brokenRules)
        {
            BrokenRules = brokenRules;
        }
    }
}