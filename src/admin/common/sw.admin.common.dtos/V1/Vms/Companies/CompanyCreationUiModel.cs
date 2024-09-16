using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.admin.common.dtos.V1.Vms.Companies
{
    public class CompanyCreationUiModel : IUiModel
    {
        [Key]
        [Editable(true)]
        public long Id { get; set; }

        public string Message { get; set; }
    }
}
