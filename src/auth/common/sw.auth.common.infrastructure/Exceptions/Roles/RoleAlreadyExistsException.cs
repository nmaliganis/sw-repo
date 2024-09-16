using System;

namespace sw.auth.common.infrastructure.Exceptions.Roles
{
    public class RoleAlreadyExistsException : Exception
    {
        public string Name { get; }
        public string BrokenRules { get; }

        public RoleAlreadyExistsException(string name, string brokenRules)
        {
            Name = name;
            BrokenRules = brokenRules;
        }

        public override string Message => $" Role with Name:{Name} already Exists!\n Additional info:{BrokenRules}";
    }
}