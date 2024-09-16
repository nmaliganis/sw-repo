using System.ComponentModel.DataAnnotations;
using sw.common.dtos.Vms.Users;
using sw.infrastructure.Security;

namespace sw.auth.common.dtos.Vms.Users
{
    public class AuthUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(false)]
        public string Token { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(false)]
        public string RefreshToken { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Editable(false)]
        public AuthUserDataUiModel UserData { get; set; }
    }
}