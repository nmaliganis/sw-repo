using sw.routing.model.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using FluentNHibernate.Mapping;

namespace sw.routing.repository.Mappings.ItineraryTemplates;

public class ItineraryTemplateLocationPointMap : ClassMap<ItineraryTemplateLocationPoint>
{
    public ItineraryTemplateLocationPointMap()
    {
        this.Table("`ItineraryTemplatesPoints`");

        this.Id(x => x.Id)
            .Column("id")
            .CustomType("long")
            .Access.Property()
            .Not.Nullable()
            .GeneratedBy
            .Increment()
            ;

        this.Map(x => x.IsStart)
            .Column("is_start")
            .CustomType("Boolean")
            .Access.Property()
            .Generated.Never()
            .Default("true")
            .CustomSqlType("boolean")
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

        References(x => x.Template)
            .Class<ItineraryTemplate>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("itinerary_template_id")
            ;

        References(x => x.Location)
            .Class<LocationPoint>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("point_id")
            ;
    }
}