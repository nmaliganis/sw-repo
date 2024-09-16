using sw.auth.model.Members;
using sw.auth.model.Users;
using sw.onboarding.model.Members;
using FluentNHibernate.Mapping;

namespace sw.auth.repository.Mappings.Members;

public class MemberMap : ClassMap<Member>
{
  public MemberMap()
  {
    Table("`Members`");

    Id(x => x.Id)
      .Column("id")
      .CustomType("long")
      .Access.Property()
      .Not.Nullable()
      .GeneratedBy
      .Identity()
      ;

    Map(x => x.Firstname)
      .Column("Firstname")
      .CustomType("string")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(128)")
      .Not.Nullable()
      .Length(128)
      ;

    Map(x => x.Lastname)
      .Column("Lastname")
      .CustomType("string")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(128)")
      .Not.Nullable()
      .Length(128)
      ;

    Map(x => x.Email)
      .Column("Email")
      .CustomType("string")
      .Access.Property()
      .Generated.Never()
      .Unique()
      .CustomSqlType("varchar(128)")
      .Not.Nullable()
      .Length(128)
      ;

    Map(x => x.Gender)
      .Column("gender")
      .CustomType<MemberGenderType>()
      .Access.Property()
      .Generated.Never()
      .Default(@"1")
      .CustomSqlType("integer")
      .Not.Nullable()
      ;

    Map(x => x.Phone)
      .Column("phone")
      .CustomType("string")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("nvarchar")
      .Not.Nullable()
      .Length(10)
      ;

    Map(x => x.ExtPhone)
      .Column("extphone")
      .CustomType("string")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("nvarchar")
      .Not.Nullable()
      .Length(5)
      ;

    Map(x => x.Mobile)
      .Column("mobile")
      .CustomType("string")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("nvarchar")
      .Not.Nullable()
      .Length(10)
      ;

    Map(x => x.ExtMobile)
      .Column("extmobile")
      .CustomType("string")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("nvarchar")
      .Not.Nullable()
      .Length(5)
      ;

    Map(x => x.Notes)
      .Column("notes")
      .CustomType("string")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("nvarchar")
      .Nullable()
      ;

    Map(x => x.Active)
      .Column("active")
      .CustomType("boolean")
      .Access.Property()
      .Generated.Never()
      .Default("true")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    Map(x => x.CreatedDate)
      .Column("created_date")
      .CustomType("DateTime")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.ModifiedDate)
      .Column("modified_date")
      .CustomType("DateTime")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.CreatedBy)
      .Column("created_by")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.ModifiedBy)
      .Column("modified_by")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Component(x => x.Address,
      memberAddress =>
      {
        memberAddress.Access.Property();
        memberAddress.Map(x => x.Street)
          .Column("address_street")
          .CustomType("string")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("nvarchar")
          .Not.Nullable()
          .Length(128)
          ;

        memberAddress.Map(x => x.StreetNumber)
          .Column("address_street_number")
          .CustomType("string")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("nvarchar")
          .Nullable()
          .Length(128)
          ;

        memberAddress.Map(x => x.PostCode)
          .Column("address_postcode")
          .CustomType("string")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("nvarchar")
          .Not.Nullable()
          .Length(8)
          ;

        memberAddress.Map(x => x.City)
          .Column("address_city")
          .CustomType("string")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("nvarchar")
          .Not.Nullable()
          .Length(64)
          ;

        memberAddress.Map(x => x.Region)
          .Column("address_region")
          .CustomType("string")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("nvarchar")
          .Not.Nullable()
          .Length(64)
          ;
      });

    References(x => x.User)
      .Class<User>()
      .Access.Property()
      .Cascade.SaveUpdate()
      .Fetch.Join()
      .Columns("user_id")
      ;
  }
}