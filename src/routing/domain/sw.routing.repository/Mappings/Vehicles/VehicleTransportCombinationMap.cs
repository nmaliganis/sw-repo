using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.model.Jobs;
using sw.routing.model.TransportCombinations;
using sw.routing.model.Vehicles;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace sw.routing.repository.Mappings.Vehicles;

public class VehicleTransportCombinationMap : ClassMap<VehicleTransportCombination>
{
    public VehicleTransportCombinationMap()
    {
        this.Table("`VehiclesTransportCombinations`");

        this.Id(x => x.Id)
          .Column("id")
          .CustomType("long")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy
          .Increment()
          ;

        this.Map(x => x.Type)
            .Column("type")
            .CustomType<TransportCombinationType>()
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

        References(x => x.Vehicle)
            .Class<Vehicle>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("vehicle_id")
            ;

        References(x => x.TransportCombination)
            .Class<TransportCombination>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("transport_combination_id")
            ;
    }
}