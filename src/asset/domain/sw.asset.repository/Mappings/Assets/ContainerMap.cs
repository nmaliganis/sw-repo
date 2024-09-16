using sw.asset.model.Assets.Containers;
using sw.asset.model.Assets.Containers.Types;
using sw.asset.model.Companies.Zones;
using sw.asset.repository.Mappings.Base.CustomeTypes;
using FluentNHibernate.Mapping;

namespace sw.asset.repository.Mappings.Assets;

public class ContainerMap : SubclassMap<Container>
{
    public ContainerMap()
    {
        DiscriminatorValue("container");
        this.Table("`Container`");
        Abstract();
        KeyColumn("id");

        this.Map(x => x.IsVisible)
          .Column("isvisible")
          .CustomType("Boolean")
          .Access.Property()
          .Generated.Never()
          .Default("true")
          .CustomSqlType("boolean")
          .Not.Nullable()
          ;

        this.Map(x => x.Level)
          .Column("level")
          .Access.Property()
          .Generated.Never()
          .Not.Nullable()
          ;

        this.Map(x => x.PrevLevel)
            .Column("prevlevel")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        Map(m => m.GeoPoint, "point")
          .CustomType<Wgs84GeographyType>()
          .LazyLoad()
          .Nullable()
          ;

        this.Map(x => x.TimeToFull)
          .Column("timetofull")
          .CustomType("double")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.LastServicedDate)
          .Column("lastserviceddate")
          .Access.Property()
          .Generated.Never()
          .Not.Nullable()
          ;

        this.Map(x => x.Capacity)
            .Column("capacity")
            .CustomType<ContainerCapacity>()
            .Access.Property()
            .Generated.Never()
            .Default(@"1")
            .CustomSqlType("integer")
            .Not.Nullable()
            ;

        this.Map(x => x.ContainerStatus)
          .Column("status")
          .CustomType<ContainerStatus>()
          .Access.Property()
          .Generated.Never()
          .Default(@"1")
          .CustomSqlType("integer")
          .Not.Nullable()
          ;

        this.Map(x => x.ContainerCondition)
          .Column("binstatus")
          .CustomType<ContainerCondition>()
          .Access.Property()
          .Generated.Never()
          .Default(@"1")
          .CustomSqlType("integer")
          .Not.Nullable()
          ;

        this.Map(x => x.MandatoryPickupDate)
          .Column("mandatorypickupdate")
          .Access.Property()
          .Generated.Never()
          .Not.Nullable()
          ;

        this.Map(x => x.MandatoryPickupActive)
          .Column("mandatorypickupactive")
          .CustomType("Boolean")
          .Access.Property()
          .Generated.Never()
          .Default("false")
          .CustomSqlType("boolean")
          .Not.Nullable()
          ;

        this.Map(x => x.WasteType)
          .Column("wastetype")
          .CustomType<ContainerType>()
          .Access.Property()
          .Generated.Never()
          .Default(@"1")
          .CustomSqlType("integer")
          .Not.Nullable()
          ;

        this.Map(x => x.Material)
          .Column("material")
          .CustomType<MaterialType>()
          .Access.Property()
          .Generated.Never()
          .Default(@"1")
          .CustomSqlType("integer")
          .Not.Nullable()
          ;

        this.References(x => x.Zone)
            .Class<Zone>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("`zoneId`")
            ;
    }
}// Class: ContainerMap 