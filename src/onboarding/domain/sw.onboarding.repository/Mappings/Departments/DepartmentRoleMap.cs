using sw.auth.model.Departments;
using sw.auth.model.Roles;
using sw.onboarding.model.Departments;
using FluentNHibernate.Mapping;

namespace sw.auth.repository.Mappings.Departments;

public class DepartmentRoleMap : ClassMap<DepartmentRole> {
  public DepartmentRoleMap() {
    this.Table("`DepartmentsRoles`");

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

    this.References(x => x.Role)
      .Class<Role>()
      .Access.Property()
      .Cascade.SaveUpdate()
      .LazyLoad()
      .Columns("role_id")
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