using System;

namespace sw.auth.common.infrastructure.Exceptions.Users {
    public class UserDoesNotExistException : Exception {
        public long UserId { get; }

        public UserDoesNotExistException(long userId) {
            this.UserId = userId;
        }

        public override string Message => $"User with Id: {this.UserId}  doesn't exists!";
    }
}