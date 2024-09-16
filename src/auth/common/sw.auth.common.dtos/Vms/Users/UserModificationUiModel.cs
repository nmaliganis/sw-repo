using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Users
{
    public class UserModificationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ModifiedName { get; set; }
    }
}