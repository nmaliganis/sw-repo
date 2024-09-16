using sw.auth.model.Companies;
using sw.auth.model.Departments;
using sw.auth.model.Members;
using FluentNHibernate.Mapping;

namespace sw.auth.repository.Mappings.Departments;

public class DepartmentMap : ClassMap<Department>
{
    public DepartmentMap()
    {
        this.Table("`Departments`");

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
            .Length(250)
            ;

        this.Map(x => x.Notes)
            .Column("notes")
            .CustomType("String")
            .Access.Property()
            .Generated.Never()
            .Nullable()
            ;

        this.Map(x => x.CodeErp)
            .Column("`codeErp`")
            .CustomType("String")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            .Length(150)
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

        this.HasMany<MemberDepartment>(x => x.Members)
            .Access.Property()
            .AsSet()
            .Cascade.All()
            .LazyLoad()
            .Inverse()
            .Generic()
            .KeyColumns.Add("department_id", mapping => mapping.Name("department_id")
                .Not.Nullable())
            ;

        this.HasMany<DepartmentRole>(x => x.Roles)
            .Access.Property()
            .AsSet()
            .Cascade.All()
            .LazyLoad()
            .Inverse()
            .Generic()
            .KeyColumns.Add("role_id", mapping => mapping.Name("role_id")
                .Not.Nullable())
            ;

        this.References(x => x.Company)
            .Class<Company>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("company_id");
    }
}