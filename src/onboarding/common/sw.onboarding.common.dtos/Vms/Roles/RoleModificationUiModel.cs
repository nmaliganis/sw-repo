using System.ComponentModel.DataAnnotations;

namespace sw.auth.common.dtos.Vms.Roles
{
    public class RoleModificationUiModel
    {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ModifiedName { get; set; }

        [Key]
        public long Id { get; set; }

        public object Message { get; set; }
    }
}