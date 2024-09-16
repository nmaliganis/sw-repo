namespace sw.asset.common.dtos.ResourceParameters.Assets;

public abstract class AssetBaseResourceParameters
{
    public long CompanyId { get; set; }

    public long AssetCategoryId { get; set; }

    public string Name { get; set; }

    public string CodeErp { get; set; }

    public string Image { get; set; }

    public string Description { get; set; }
}