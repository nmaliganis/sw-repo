using sw.routing.model.CustomTypes;
using sw.routing.model.ItineraryTemplates;
using sw.routing.model.ItineraryTemplates.LocationPoints;
using sw.routing.repository.Mappings.Base.CustomTypes;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace sw.routing.repository.Mappings.Points;

public class LocationPointMap : ClassMap<LocationPoint>
{
    public LocationPointMap()
    {
        this.Table("`Points`");

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
          .CustomSqlType("varchar(128)")
          .Not.Nullable()
          .Length(128)
          ;

        Map(m => m.Location, "location")
            .CustomType<Wgs84GeographyType>()
            .LazyLoad()
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

        this.HasMany(x => x.Templates)
          .Access.Property()
          .AsSet()
          .Cascade.All()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("point_id", mapping => mapping.Name("point_id")
            .Not.Nullable())
          ;
    }
}