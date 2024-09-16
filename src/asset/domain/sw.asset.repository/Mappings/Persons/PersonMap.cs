using sw.asset.model.Persons;
using FluentNHibernate.Mapping;

namespace sw.asset.repository.Mappings.Persons;

public class PersonMap : ClassMap<Person>
{
  public PersonMap()
  {
    this.Id(x => x.Id)
      .Column("id")
      .CustomType("long")
      .Access.Property()
      .Not.Nullable()
      .GeneratedBy.Native()
      ;
    this.Map(x => x.FirstName)
      .Column("firstname")
      .CustomType("String")
      .Unique()
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(250)")
      .Not.Nullable()
      .Length(250)
      ;

    this.Map(x => x.LastName)
      .Column("lastname")
      .CustomType("String")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(250)")
      .Not.Nullable()
      .Length(250)
      ;

    this.Map(x => x.Email)
      .Column("email")
      .CustomType("String")
      .Access.Property()
      .Generated.Never()
      .Unique()
      .CustomSqlType("varchar(250)")
      .Not.Nullable()
      .Length(250)
      ;

    this.Map(x => x.Username)
      .Column("username")
      .CustomType("String")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(250)")
      .Not.Nullable()
      .Length(250)
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

    this.HasMany(x => x.Departments)
      .Access.Property()
      .AsSet()
      .Cascade.All()
      .LazyLoad()
      .Inverse()
      .Generic()
      .KeyColumns.Add("`departmentId`", mapping =>
        mapping.Name("`departmentId`")
          .Not.Nullable())
      ;
  }
}// Class: PersonMap