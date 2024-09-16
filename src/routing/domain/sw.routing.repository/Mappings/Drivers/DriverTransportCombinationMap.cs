using sw.routing.model.Drivers;
using sw.routing.model.Itineraries;
using sw.routing.model.Jobs;
using sw.routing.model.TransportCombinations;
using sw.routing.model.Vehicles;
using FluentNHibernate.Mapping;

namespace sw.routing.repository.Mappings.Drivers;

public class DriverTransportCombinationMap : ClassMap<DriverTransportCombination>
{
    public DriverTransportCombinationMap()
    {
        this.Table("`DriversTransportCombinations`");

        this.Id(x => x.Id)
          .Column("id")
          .CustomType("long")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy
          .Increment()
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

        References(x => x.Driver)
            .Class<Driver>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("driver_id")
            ;

        References(x => x.TransportCombination)
            .Class<TransportCombination>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("transport_combination_id")
            ;

        this.HasOne(x => x.Itinerary)
            .Class<Itinerary>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .PropertyRef(p => p.DriverTransportCombination)
            ;
    }
}