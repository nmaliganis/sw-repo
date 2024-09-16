using System.ComponentModel.DataAnnotations;

namespace sw.infrastructure.DTOs.Base
{
    public interface IUiModel
    {
        [Key]
        long Id { get; set; }
        [Editable(false)]
        string Message { get; set; }
    }
}