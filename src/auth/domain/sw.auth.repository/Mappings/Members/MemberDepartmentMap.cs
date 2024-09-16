using sw.auth.model.Departments;
using sw.auth.model.Members;
using FluentNHibernate.Mapping;

namespace sw.auth.repository.Mappings.Members;

public class MemberDepartmentMap : ClassMap<MemberDepartment>
{
    public MemberDepartmentMap()
    {
        this.Table("`MembersDepartments`");

        this.Id(x => x.Id)
          .Column("id")
          .CustomType("long")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy
          .Identity()
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

        this.References(x => x.Member)
          .Class<Member>()
          .Access.Property()
          .Cascade.SaveUpdate()
          .LazyLoad()
          .Columns("member_id")
          ;

        this.References(x => x.Department)
          .Class<Department>()
          .Access.Property()
          .Cascade.SaveUpdate()
          .LazyLoad()
          .Columns("department_id")
          ;
    }
}