using sw.auth.model.Users;
using FluentNHibernate.Mapping;

namespace sw.auth.repository.Mappings.Users;

public class RefreshTokenMap : ClassMap<RefreshToken>
{
  public RefreshTokenMap()
  {
    this.Table("`RefreshTokens`");

    this.Id(x => x.Id)
      .Column("id")
      .CustomType("long")
      .Access.Property()
      .Not.Nullable()
      .GeneratedBy
      .Identity()
      ;

    this.Map(x => x.JwtToken)
      .Column("token")
      .CustomType("Guid")
      .Access.Property()
      .Unique()
      .Generated.Never()
      .CustomSqlType("UUID")
      .Not.Nullable()
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

    this.Map(x => x.Expired)
      .Column("expired")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("false")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    this.References(x => x.User)
      .Class<User>()
      .Access.Property()
      .Cascade.SaveUpdate()
      .LazyLoad()
      .Columns("user_id");
  }
}//Class : RefreshTokenMap