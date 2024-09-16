using System;

namespace sw.azure.messaging.Models.IoT
{
  public abstract class BaseIoT
  {
    public string Imei { get; set; }
    public DateTime Recorded { get; set; }
  }
}