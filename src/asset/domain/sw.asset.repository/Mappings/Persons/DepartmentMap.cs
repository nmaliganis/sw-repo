using sw.asset.model.Companies;
using sw.asset.model.Persons;
using FluentNHibernate.Mapping;

namespace sw.asset.repository.Mappings.Persons;

public class DepartmentMap : ClassMap<Department>
{
	public DepartmentMap()
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

        this.Map(x => x.Notes)
          .Column("notes")
          .CustomType("String")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("varchar(250)")
          .Not.Nullable()
          .Length(250)
          ;

        this.References(x => x.Company)
          .Class<Company>()
          .Access.Property()
          .Cascade.SaveUpdate()
          .LazyLoad()
          .Columns("`companyId`")
          ;

        this.HasMany(x => x.Persons)
          .Access.Property()
          .AsSet()
          .Cascade.All()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("`personId`", mapping =>
            mapping.Name("`personId`")
              .Not.Nullable());
  }
}// Class: DepartmentMap