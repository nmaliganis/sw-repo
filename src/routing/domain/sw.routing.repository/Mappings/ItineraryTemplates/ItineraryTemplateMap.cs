using sw.routing.model.CustomTypes;
using sw.routing.model.ItineraryTemplates;
using sw.routing.repository.Mappings.Base.CustomTypes;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace sw.routing.repository.Mappings.ItineraryTemplates;

public class ItineraryTemplateMap : ClassMap<ItineraryTemplate>
{
    public ItineraryTemplateMap()
    {
        this.Table("`ItineraryTemplates`");

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
          .Length(128)
          ;

        Map(x => x.Zones)
            .CustomSqlType("jsonb")
            .CustomType<JsonType<ZoneJsonbType>>()
            .Column("zones")
            .Nullable()
            ;

        Map(x => x.MinFillLevel)
            .Column("min_fill_level")
            .CustomType("double")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;


        Map(x => x.Occurrence)
            .CustomSqlType("jsonb")
            .CustomType<JsonType<OccurrenceJsonbType>>()
            .Column("occurrence")
            .Nullable()
            ;

        this.Map(x => x.StartTime)
            .CustomType<NHibernate.Type.TimeAsTimeSpanType>()
            .Column("start_time")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.Stream)
            .Column("stream")
            .CustomType<StreamStringType>()
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.Description)
            .Column("description")
            .CustomType("String")
            .Access.Property()
            .Generated.Never()
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

        this.HasMany(x => x.Itineraries)
          .Access.Property()
          .AsSet()
          .Cascade.All()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("template_id", mapping => mapping.Name("template_id")
            .Not.Nullable())
          ;

        this.HasMany(x => x.Locations)
            .Access.Property()
            .AsSet()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Inverse()
            .Generic()
            .KeyColumns.Add("itinerary_template_id", mapping => mapping.Name("itinerary_template_id")
                .Not.Nullable())
            ;
    }
}