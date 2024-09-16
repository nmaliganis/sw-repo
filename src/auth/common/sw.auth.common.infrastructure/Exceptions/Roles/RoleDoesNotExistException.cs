using System;

namespace sw.auth.common.infrastructure.Exceptions.Roles
{
    public class RoleDoesNotExistException : Exception
    {
        public long RoleId { get; }

        public RoleDoesNotExistException(long roleId)
        {
            RoleId = roleId;
        }

        public override string Message => $"Role with Id: {RoleId}  doesn't exists!";
    }
}