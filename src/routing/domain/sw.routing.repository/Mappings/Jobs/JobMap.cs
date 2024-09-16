using sw.routing.model.CustomTypes;
using sw.routing.model.Itineraries;
using sw.routing.model.Jobs;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace sw.routing.repository.Mappings.Jobs;

public class JobMap : ClassMap<Job>
{
    public JobMap()
    {
        this.Table("`Jobs`");

        this.Id(x => x.Id)
            .Column("id")
            .CustomType("long")
            .Access.Property()
            .Not.Nullable()
            .GeneratedBy
            .Increment()
            ;

        this.Map(x => x.Seq)
            .Column("seq")
            .CustomType("String")
            .Access.Property()
            .Generated.Never()
            .CustomSqlType("varchar(512)")
            .Not.Nullable()
            .Length(512)
            ;

        this.Map(x => x.Index)
            .Column("index")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.Container)
            .Column("container")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        Map(x => x.Config)
            .CustomSqlType("jsonb")
            .CustomType<JsonType<ConfigJsonbType>>()
            .Column("config")
            .Nullable()
            ;

        this.Map(x => x.Arrival)
            .Column("arrival")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.Departure)
            .Column("departure")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.EstimatedArrival)
            .Column("estimated_arrival")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.EstimatedDeparture)
            .Column("estimated_departure")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.ScheduledArrival)
            .Column("scheduled_arrival")
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

        References(x => x.Itinerary)
            .Class<Itinerary>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("itinerary_id")
            ;

        References(x => x.Parent)
            .Class<Job>()
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
    }
}