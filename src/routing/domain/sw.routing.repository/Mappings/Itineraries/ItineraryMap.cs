using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.model.ItineraryTemplates;
using sw.routing.model.TransportCombinations;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace sw.routing.repository.Mappings.Itineraries;

public class ItineraryMap : ClassMap<Itinerary>
{
    public ItineraryMap()
    {
        this.Table("`Itineraries`");

        this.Id(x => x.Id)
          .Column("id")
          .CustomType("long")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy
          .Increment()
          ;

        this.Map(x => x.Name)
            .Column("name")
            .CustomType("String")
            .Unique()
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(512)")
            .Not.Nullable()
            .Length(512)
            ;

        Map(x => x.Config)
            .CustomSqlType("jsonb")
            .CustomType<JsonType<ConfigJsonbType>>()
            .Column("config")
            .Nullable()
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

        References(x => x.Template)
            .Class<ItineraryTemplate>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("template_id")
            ;

        References(x => x.DriverTransportCombination)
            .Class<DriverTransportCombination>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .Fetch.Join()
            .Columns("driver_transport_combination_id")
            ;

        References(x => x.Parent)
            .Class<Itinerary>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("correlation_id")
            ;

        this.HasMany(x => x.Children)
          .Access.Property()
          .AsSet()
          .Cascade.All()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("correlation_id", mapping => mapping.Name("correlation_id")
            .Nullable())
          ;

        this.HasMany(x => x.Jobs)
            .Access.Property()
            .AsSet()
            .Cascade.All()
            .LazyLoad()
            .Inverse()
            .Generic()
            .KeyColumns.Add("itinerary_id", mapping => mapping.Name("itinerary_id")
                .Not.Nullable())
            ;
    }
}