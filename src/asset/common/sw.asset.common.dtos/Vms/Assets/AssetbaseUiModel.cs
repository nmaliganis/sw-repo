using System.ComponentModel.DataAnnotations;
using sw.infrastructure.DTOs.Base;

namespace sw.asset.common.dtos.Vms.Assets;

public abstract class AssetBaseUiModel : IUiModel
{
    public string Message { get; set; }

    [Key]
    [Editable(true)]
    public long Id { get; set; }

    [Editable(true)]
    public long CompanyId { get; set; }

    [Editable(true)]
    public long AssetCategoryId { get; set; }


    [Editable(true)]
    [StringLength(250)]
    public string Name { get; set; }

    [Editable(true)]
    [StringLength(150)]
    public string CodeErp { get; set; }

    [Editable(true)]
    [StringLength(250)]
    public string Image { get; set; }

    [Editable(true)]
    public string Description { get; set; }
}