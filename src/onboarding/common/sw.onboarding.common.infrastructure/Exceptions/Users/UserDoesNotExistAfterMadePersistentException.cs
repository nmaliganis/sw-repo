using System;

namespace sw.auth.common.infrastructure.Exceptions.Users
{
    public class UserDoesNotExistAfterMadePersistentException : Exception
    {
        public string Login { get; }
        public string Email { get; private set; }

        public UserDoesNotExistAfterMadePersistentException(string login, string email)
        {
            Login = login;
            Email = email;
        }

        public override string Message => $" User with login or/and Email: {Email} was not made Persistent!";
    }
}