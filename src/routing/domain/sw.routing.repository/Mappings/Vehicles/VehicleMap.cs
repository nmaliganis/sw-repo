using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.model.Jobs;
using sw.routing.model.Vehicles;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace sw.routing.repository.Mappings.Vehicles;

public class VehicleMap : ClassMap<Vehicle>
{
    public VehicleMap()
    {
        this.Table("`Vehicles`");

        this.Id(x => x.Id)
          .Column("id")
          .CustomType("long")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy
          .Identity()
          ;

        this.Map(x => x.NumPlate)
            .Column("numplate")
            .CustomType("String")
            .Access.Property()
            .Unique()
            .Generated.Never()
            .CustomSqlType("varchar(16)")
            .Not.Nullable()
            .Length(16)
            ;

        this.Map(x => x.Brand)
            .Column("brand")
            .CustomType("String")
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(32)")
            .Not.Nullable()
            .Length(32)
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
            .KeyColumns.Add("vehicle_id", mapping => mapping.Name("vehicle_id")
                .Nullable())
            ;
    }
}