using sw.asset.model.Events;
using sw.asset.model.Sensors;
using sw.infrastructure.CustomTypes;
using FluentNHibernate.Mapping;
using nhibernate.postgresql.json;

namespace sw.asset.repository.Mappings.Events;

public class EventHistoryMap : ClassMap<EventHistory>
{
    public EventHistoryMap()
    {
        Table(@"`EventHistory`");
        Id(x => x.Id)
          .Column("id")
          .Access.Property()
          .Not.Nullable()
          .GeneratedBy
          .Guid()
          ;

        this.Map(x => x.EventValue)
          .CustomSqlType("jsonb")
          .CustomType<JsonType<JsonType>>()
          .Column("eventvaluejson")
          .Nullable()
          ;

        this.Map(x => x.Recorded)
            .Column("recorded")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.Received)
            .Column("received")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.Map(x => x.Value)
            .Column("eventvalue")
            .Access.Property()
            .Generated.Never()
            .Not.Nullable()
            ;

        this.References(x => x.Sensor)
            .Class<Sensor>()
            .Access.Property()
            .Cascade.SaveUpdate()
            .LazyLoad()
            .Columns("`sensorId`")
            ;
    }
}// Class: SensorMap