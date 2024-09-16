using FluentNHibernate.Mapping;
using sw.asset.model.Assets.Categories;

namespace sw.asset.repository.Mappings.AssetCategories;

public class AssetCategoryMap : ClassMap<AssetCategory>
{
  public AssetCategoryMap()
  {
    this.Id(x => x.Id)
      .Column("id")
      .CustomType("long")
      .Access.Property()
      .Not.Nullable()
      .GeneratedBy
      .Identity()
      ;

    this.Map(x => x.Name)
      .Column("name")
      .CustomType("String")
      .Unique()
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(250)")
      .Not.Nullable()
      .Length(250)
      ;

    this.Map(x => x.CodeErp)
      .Column("`codeErp`")
      .CustomType("String")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(150)")
      .Not.Nullable()
      .Length(150)
      ;

    this.Map(x => x.CreatedBy)
      .Column("created_by")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.ModifiedBy)
      .Column("modified_by")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.DeletedBy)
      .Column("deleted_by")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.CreatedDate)
      .Column("created_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.ModifiedDate)
      .Column("modified_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.DeletedDate)
      .Column("deleted_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    this.Map(x => x.DeletedReason)
      .Column("deleted_reason")
      .Access.Property()
      .Generated.Never()
      .Nullable()
      ;

    this.Map(x => x.Active)
      .Column("active")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("true")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    this.HasMany(x => x.Assets)
      .Access.Property()
      .AsSet()
      .Cascade.SaveUpdate()
      .LazyLoad()
      .Inverse()
      .Generic()
      .KeyColumns.Add("assetcategory_id", mapping =>
        mapping.Name("assetcategory_id")
          .Not.Nullable())
      ;
  }
}// Class: AssetCategoryMap
