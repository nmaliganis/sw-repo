using sw.asset.model.Sensors;
using sw.infrastructure.CustomTypes;
using sw.infrastructure.Domain;
using System;

namespace sw.asset.model.Events;

public class EventHistory : EntityBase<Guid>, IAggregateRoot
{
    public EventHistory()
    {
        this.OnCreated();
    }

    private void OnCreated()
    {
    }

    public virtual DateTime Recorded { get; set; }
    public virtual DateTime Received { get; set; }
    public virtual double Value { get; set; }
    public virtual JsonType EventValue { get; set; }

    public virtual Sensor Sensor { get; set; }

    protected override void Validate()
    {

    }

    public virtual void InjectWithParams(string eventValueJson)
    {
        this.EventValue = new JsonType()
        {
            Params = eventValueJson
        };
    }

    public virtual void InjectWithSensor(Sensor sensorToBeInjected)
    {
        this.Sensor = sensorToBeInjected;
        //sensorToBeInjected.Events.Add(this);
    }
}// Class: EventHistory