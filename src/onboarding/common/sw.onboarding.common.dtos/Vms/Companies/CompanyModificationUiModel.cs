using System.ComponentModel.DataAnnotations;

namespace sw.common.dtos.Vms.Companies {
    public class CompanyModificationUiModel {
        [Required(AllowEmptyStrings = false)]
        [Editable(true)]
        public string ModifiedName { get; set; }
        [Key]
        public long Id { get; set; }
        public object ModifiedDescription { get; set; }
        public object ModifiedCodeErp { get; set; }
        public object Message { get; set; }
    }
}