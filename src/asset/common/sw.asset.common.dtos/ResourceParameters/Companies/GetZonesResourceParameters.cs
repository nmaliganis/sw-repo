using sw.infrastructure.ResourseParameters;

namespace sw.asset.common.dtos.ResourceParameters.Companies;

public class GetZonesResourceParameters : BaseResourceParameters
{
    /// <summary>
    /// <param name="Filter">Filter in Field
    /// (id, login,  createdBy, createDate e.t.c.)</param>
    /// </summary>
    public override string Filter { get; set; }
    /// <summary>
    /// <param name="SearchQuery">Search into Fields
    /// (id, login, createdBy, createDate e.t.c.)</param>
    /// </summary>
    public override string SearchQuery { get; set; }
    /// <summary>
    /// <param name="Fields">Fields to be Shown
    /// (id, login, createdBy, createDate e.t.c.)</param>
    /// </summary>
    public override string Fields { get; set; }
    /// <summary>
    /// <param name="OrderBy">Order by specific field
    /// (id, name, createdBy, createDate e.t.c.)</param>
    /// </summary>
    public override string OrderBy { get; set; } = "Id";
}