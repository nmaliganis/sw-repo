using sw.auth.model.Roles;
using FluentNHibernate.Mapping;

namespace sw.auth.repository.Mappings.Roles;

public class RoleMap : ClassMap<Role>
{
  public RoleMap()
  {
    this.Table("`Roles`");

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
      .CustomSqlType("varchar(128)")
      .Not.Nullable()
      .Length(128);

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

    this.HasMany(x => x.Departments)
      .Access.Property()
      .AsSet()
      .Cascade.All()
      .LazyLoad()
      .Inverse()
      .Generic()
      .KeyColumns.Add("role_id", mapping => mapping.Name("role_id")
        .Not.Nullable())
      ;
  }
}