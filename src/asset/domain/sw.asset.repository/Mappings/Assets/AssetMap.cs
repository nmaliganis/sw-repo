using sw.asset.model.Assets;
using sw.asset.model.Assets.Categories;
using sw.asset.model.Companies;
using FluentNHibernate.Mapping;

namespace sw.asset.repository.Mappings.Assets;
public class AssetMap : ClassMap<Asset>
{
    public AssetMap()
    {
        this.Id(x => x.Id)
          .Column("id")
          .CustomType("long")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy.Native()
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

        this.Map(x => x.Image)
          .Column("image")
          .CustomType("String")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("varchar(250)")
          .Nullable()
          .Length(250)
          ;

        this.Map(x => x.CodeErp)
          .Column("codeerp")
          .CustomType("String")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("varchar(150)")
          .Nullable()
          .Length(150)
          ;

        this.Map(x => x.Description)
          .Column("description")
          .CustomType("String")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.CreatedBy)
          .Column("created_by")
          .CustomType("long")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.ModifiedBy)
          .Column("modified_by")
          .CustomType("long")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.DeletedBy)
          .Column("deleted_by")
          .CustomType("long")
          .Access.Property()
          .Generated.Never()
          .Nullable()
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

        this.References(x => x.Company)
          .Class<Company>()
          .Access.Property()
          .Cascade.None()
          .LazyLoad()
          .Columns("`companyId`")
          ;

        this.References(x => x.AssetCategory)
          .Class<AssetCategory>()
          .Access.Property()
          .Cascade.None()
          .LazyLoad()
          .Columns("assetcategory_id")
          ;

        this.HasMany(x => x.Sensors)
          .Access.Property()
          .AsList()
          .Cascade.All()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("`assetId`", mapping =>
            mapping.Name("`assetId`")
              .Not.Nullable())
          ;
    }
}// Class: AssetMap