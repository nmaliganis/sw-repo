using sw.asset.model.Persons;
using FluentNHibernate.Mapping;

namespace sw.asset.repository.Mappings.Persons;

public class DepartmentPersonMap : ClassMap<DepartmentPerson>
{
  public DepartmentPersonMap()
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

    this.References(x => x.Department)
        .Class<Department>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .LazyLoad()
        .Columns("`departmentId`")
        ;

    this.References(x => x.Person)
        .Class<Person>()
        .Access.Property()
        .Cascade.SaveUpdate()
        .LazyLoad()
        .Columns("person_id")
        ;
  }
}// Class: DepartmentPersonMap