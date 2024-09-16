using System;

namespace sw.auth.common.infrastructure.Exceptions.Roles
{
    public class RoleDoesNotExistAfterMadePersistentException : Exception
    {
        public string Name { get; private set; }

        public RoleDoesNotExistAfterMadePersistentException(string name)
        {
            Name = name;
        }

        public override string Message => $" Role with Name: {Name} was not made Persistent!";
    }
}