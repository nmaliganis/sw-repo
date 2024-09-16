using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.Vms.Accounts
{
    public class LoginUiModel
    {
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string Password { get; set; }
    }
}