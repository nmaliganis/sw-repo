using sw.auth.model.Members;
using sw.auth.model.Users;
using FluentNHibernate.Mapping;

namespace sw.auth.repository.Mappings.Users;

public class UserMap : ClassMap<User>
{
  public UserMap()
  {
    this.Table("`Users`");

    this.Id(x => x.Id)
      .Column("id")
      .CustomType("long")
      .Access.Property()
      .Not.Nullable()
      .GeneratedBy
      .Identity()
      ;

    this.Map(x => x.Login)
      .Column("login")
      .CustomType("String")
      .Unique()
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(512)")
      .Not.Nullable()
      .Length(512)
      ;

    this.Map(x => x.PasswordHash)
      .Column("password_hash")
      .CustomType("String")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(512)")
      .Not.Nullable()
      .Length(512)
      ;

    this.Map(x => x.IsActivated)
      .Column("is_activated")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("false")
      .CustomSqlType("boolean")
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

    this.Map(x => x.ResetKey)
      .Column("reset_key")
      .CustomType("Guid")
      .Unique()
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("UUID")
      .Not.Nullable()
      ;

    this.Map(x => x.ActivationKey)
      .Column("activation_key")
      .CustomType("Guid")
      .Unique()
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("UUID")
      .Not.Nullable()
      ;

    this.Map(x => x.ResetDate)
      .Column("reset_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
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

    this.HasOne(x => x.Member)
      .Class<Member>()
      .Access.Property()
      .Cascade.SaveUpdate()
      .LazyLoad()
      .PropertyRef(p => p.User)
      ;

    this.HasMany(x => x.RefreshTokens)
      .Access.Property()
      .AsSet()
      .Cascade.All()
      .LazyLoad()
      .Inverse()
      .Generic()
      .KeyColumns.Add("user_id", mapping => mapping.Name("user_id")
        .Not.Nullable())
      ;
  }
}//Class : UserMap