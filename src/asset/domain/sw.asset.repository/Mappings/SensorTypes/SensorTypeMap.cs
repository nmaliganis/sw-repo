using sw.asset.model.SensorTypes;
using FluentNHibernate.Mapping;

namespace sw.asset.repository.Mappings.SensorTypes;

public class SensorTypeMap : ClassMap<SensorType>
{
  public SensorTypeMap()
  {
    Id(x => x.Id)
      .Column("id")
      .CustomType("long")
      .Access.Property()
      .Not.Nullable()
      .GeneratedBy
      .Identity()
      ;

    Map(x => x.SensorTypeIndex)
      .Column("sensortypeindex")
      .CustomType("int")
      .Access.Property()
      .Default("0")
      .Unique()
      .CustomSqlType("integer")
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.Name)
      .Column("name")
      .CustomType("String")
      .Unique()
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(250)")
      .Not.Nullable()
      .Length(250)
      ;

    Map(x => x.ShowAtStatus)
      .Column("showatstatus")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("false")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    Map(x => x.StatusExpiryMinutes)
      .Column("statusexpiryminutes")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Default("0")
      .Not.Nullable()
      ;

    Map(x => x.ShowOnMap)
      .Column("showonmap")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("false")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    Map(x => x.ShowAtReport)
      .Column("showatreport")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("false")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    Map(x => x.ShowAtChart)
      .Column("showatchart")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("false")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    Map(x => x.ResetValues)
      .Column("resetvalues")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("false")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    Map(x => x.SumValues)
      .Column("sumvalues")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("false")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    Map(x => x.Precision)
      .Column("precision")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Default("0")
      .Not.Nullable()
      ;

    Map(x => x.Tunit)
      .Column("tunit")
      .CustomType("String")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(100)")
      .Nullable()
      .Length(100)
      ;

    Map(x => x.CodeErp)
      .Column("`codeErp`")
      .CustomType("String")
      .Access.Property()
      .Generated.Never()
      .CustomSqlType("varchar(150)")
      .Not.Nullable()
      .Length(150)
      ;

    Map(x => x.CreatedBy)
      .Column("created_by")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.ModifiedBy)
      .Column("modified_by")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.DeletedBy)
      .Column("deleted_by")
      .CustomType("long")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.CreatedDate)
      .Column("created_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.ModifiedDate)
      .Column("modified_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.DeletedDate)
      .Column("deleted_date")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.DeletedReason)
      .Column("deleted_reason")
      .Access.Property()
      .Generated.Never()
      .Not.Nullable()
      ;

    Map(x => x.Active)
      .Column("active")
      .CustomType("Boolean")
      .Access.Property()
      .Generated.Never()
      .Default("true")
      .CustomSqlType("boolean")
      .Not.Nullable()
      ;

    this.HasMany(x => x.Sensors)
      .Access.Property()
      .AsSet()
      .Cascade.None()
      .Not.KeyUpdate()
      .LazyLoad()
      .Inverse()
      .Generic()
      .KeyColumns.Add("`sensorTypeId`", mapping =>
        mapping.Name("`sensorTypeId`")
          .Not.Nullable())
      ;
  }
}// Class: SensorTypeMap