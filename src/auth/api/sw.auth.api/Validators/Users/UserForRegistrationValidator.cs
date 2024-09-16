using sw.auth.common.dtos.Vms.Accounts;
using sw.common.dtos.Vms.Accounts;
using FluentValidation;

namespace sw.auth.api.Validators.Users
{
    /// <summary>
    /// Class : UserForRegistrationValidator
    /// </summary>
    public class UserForRegistrationValidator : AbstractValidator<UserForRegistrationUiModel>
    {
        /// <summary>
        /// Constructor : UserForRegistrationValidator
        /// </summary>
        public UserForRegistrationValidator()
        {
            RuleFor(x => x.Login).NotNull();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).Length(4, 12);
        }
    }// Class : UserForRegistrationValidator
    
}// Namespace : sw.auth.api.Validators.Users