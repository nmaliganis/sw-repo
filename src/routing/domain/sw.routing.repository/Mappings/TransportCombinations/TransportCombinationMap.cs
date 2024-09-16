using sw.routing.model.Drivers;
using sw.routing.model.TransportCombinations;
using FluentNHibernate.Mapping;

namespace sw.routing.repository.Mappings.TransportCombinations;

public class TransportCombinationMap : ClassMap<TransportCombination>
{
    public TransportCombinationMap()
    {
        this.Table("`TransportCombinations`");

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

        this.HasMany(x => x.Vehicles)
            .Access.Property()
            .AsSet()
            .Cascade.All()
            .LazyLoad()
            .Inverse()
            .Generic()
            .KeyColumns.Add("transport_combination_id",
                mapping => mapping.Name("transport_combination_id")
                    .Not.Nullable())
            ;

        this.HasMany(x => x.DriverItineraries)
            .Access.Property()
            .AsSet()
            .Cascade.All()
            .LazyLoad()
            .Inverse()
            .Generic()
            .KeyColumns.Add("transport_combination_id", 
                mapping => mapping.Name("transport_combination_id")
                .Not.Nullable())
            ;
    }
}