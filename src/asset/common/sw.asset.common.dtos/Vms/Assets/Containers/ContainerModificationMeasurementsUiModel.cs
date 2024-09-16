using System;

namespace sw.asset.common.dtos.Vms.Assets.Containers;

public class ContainerModificationMeasurementsUiModel
{
    // Base
    public DateTime Recorded { get; set; }
    //Sensor
    public double Range { get; set; }
    public double? Status { get; set; }
    public double? Temperature { get; set; }
    //GPS
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double Speed { get; set; }
    public double Direction { get; set; }
    public int FixMode { get; set; }
    public double Hdop { get; set; }
    public int SatellitesUsed { get; set; }

    // DigitalEvent
    public int PinNumber { get; set; }
    public int NewValue { get; set; }
}