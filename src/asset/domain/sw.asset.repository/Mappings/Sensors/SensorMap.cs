using sw.asset.model.Assets;
using sw.asset.model.Devices;
using sw.asset.model.Sensors;
using sw.asset.model.SensorTypes;
using sw.infrastructure.CustomTypes;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace sw.asset.repository.Mappings.Sensors;

public class SensorMap : ClassMap<Sensor>
{
    public SensorMap()
    {
        Id(x => x.Id)
          .Column("id")
          .CustomType("long")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy
          .Identity()
          ;

        this.Map(x => x.Params)
          .CustomSqlType("jsonb")
          .CustomType<JsonType<JsonType>>()
          .Column("params")
          .Nullable()
          ;

        this.Map(x => x.Name)
          .Column("name")
          .CustomType("String")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("varchar(250)")
          .Nullable()
          .Length(250)
          ;

        this.Map(x => x.CodeErp)
          .Column("`codeErp`")
          .CustomType("String")
          .Access.Property()
          .Generated.Never()
          .CustomSqlType("varchar(150)")
          .Not.Nullable()
          .Length(150)
          ;

        this.Map(x => x.IsActive)
          .Column("`isActive`")
          .CustomType("Boolean")
          .Access.Property()
          .Generated.Never()
          .Default("true")
          .CustomSqlType("boolean")
          .Not.Nullable()
          ;

        this.Map(x => x.IsVisible)
          .Column("`isVisible`")
          .CustomType("Boolean")
          .Access.Property()
          .Generated.Never()
          .Default("true")
          .CustomSqlType("boolean")
          .Nullable()
          ;

        this.Map(x => x.Order)
          .Column("`order`")
          .CustomType("long")
          .Access.Property()
          .Generated.Never()
          .Default("1")
          .Nullable()
          ;

        this.Map(x => x.MinValue)
          .Column("`minValue`")
          .CustomType("double")
          .Access.Property()
          .Default("0.0")
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.MaxValue)
          .Column("`maxValue`")
          .CustomType("double")
          .Default("0.0")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.MinNotifyValue)
          .Column("`minNotifyValue`")
          .CustomType("double")
          .Default("0.0")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.MaxNotifyValue)
          .Column("`maxNotifyValue`")
          .CustomType("double")
          .Default("0.0")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.LastValue)
          .Column("`lastValue`")
          .CustomType("double")
          .Access.Property()
          .Default("0.0")
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.LastRecordedDate)
          .Column("`lastRecordedDate`")
          .Access.Property()
          .Generated.Never()
          .Not.Nullable()
          ;

        this.Map(x => x.LastReceivedDate)
          .Column("`lastReceivedDate`")
          .Access.Property()
          .Generated.Never()
          .Not.Nullable()
          ;

        this.Map(x => x.HighThreshold)
          .Column("`highThreshold`")
          .CustomType("double")
          .Default("0.0")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.LowThreshold)
          .Column("`lowThreshold`")
          .CustomType("double")
          .Default("0.0")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.SamplingInterval)
          .Column("`samplingInterval`")
          .CustomType("long")
          .Default("15")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.ReportingInterval)
          .Column("`reportingInterval`")
          .CustomType("long")
          .Default("60")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.CreatedBy)
          .Column("created_by")
          .CustomType("long")
          .Default("1")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.ModifiedBy)
          .Column("modified_by")
          .CustomType("long")
          .Default("0")
          .Access.Property()
          .Generated.Never()
          .Nullable()
          ;

        this.Map(x => x.DeletedBy)
          .Column("deleted_by")
          .CustomType("long")
          .Access.Property()
          .Default("0")
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

        this.References(x => x.SensorType)
          .Class<SensorType>()
          .Access.Property()
          .Cascade.None()
          .LazyLoad()
          .Columns("`sensorTypeId`")
          ;

        this.References(x => x.Asset)
          .Class<Asset>()
          .Access.Property()
          .Cascade.None()
          .LazyLoad()
          .Columns("`assetId`")
          ;

        this.References(x => x.Device)
          .Class<Device>()
          .Access.Property()
          .Cascade.None()
          .LazyLoad()
          .Columns("`deviceId`")
          ;
        
        this.HasMany(x => x.Events)
          .Access.Property()
          .AsSet()
          .Cascade.None()
          .LazyLoad()
          .Inverse()
          .Generic()
          .KeyColumns.Add("`sensorId`", mapping =>
            mapping.Name("`sensorId`")
              .Not.Nullable())
          ;
    }
}// Class: SensorMap