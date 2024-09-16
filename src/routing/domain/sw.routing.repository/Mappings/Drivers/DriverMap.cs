using sw.routing.model.CustomTypes;
using sw.routing.model.Drivers;
using FluentNHibernate.Mapping;

namespace sw.routing.repository.Mappings.Drivers;

public class DriverMap : ClassMap<Driver>
{
    public DriverMap()
    {
        this.Table("`Drivers`");

        this.Id(x => x.Id)
          .Column("id")
          .CustomType("long")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy
          .Increment()
          ;

        this.Map(x => x.Firstname)
            .Column("firstname")
            .CustomType("String")
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(512)")
            .Not.Nullable()
            .Length(512)
            ;

        this.Map(x => x.Lastname)
            .Column("lastname")
            .CustomType("String")
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(512)")
            .Not.Nullable()
            .Length(512)
            ;

        this.Map(x => x.Gender)
            .Column("gender")
            .CustomType<DriverGenderType>()
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.Email)
            .Column("email")
            .CustomType("String")
            .Access.Property()
            .Unique()
            .Generated.Never()
            .CustomSqlType("varchar(512)")
            .Not.Nullable()
            .Length(512)
            ;

        this.Map(x => x.Member)
            .Column("member")
            .Access.Property()
            .Generated.Never()
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

        this.HasMany(x => x.TransportCombinations)
            .Access.Property()
            .AsSet()
            .Cascade.All()
            .LazyLoad()
            .Inverse()
            .Generic()
            .KeyColumns.Add("driver_id", mapping => mapping.Name("driver_id")
                .Nullable())
            ;
    }
}