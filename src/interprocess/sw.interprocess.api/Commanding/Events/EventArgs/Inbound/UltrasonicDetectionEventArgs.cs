using System.Collections.Generic;
using sw.azure.messaging.Models.IoT;

namespace sw.interprocess.api.Commanding.Events.EventArgs.Inbound
{
  public class UltrasonicDetectionEventArgs : System.EventArgs
  {
    public List<IoTUltrasonic> Payload { get; private set; }
    public string Imei { get; private set; }
    public bool Success { get; private set; }

    public UltrasonicDetectionEventArgs(List<IoTUltrasonic> payload, bool success, string imei)
    {
      Payload = payload;
      Imei = imei;
      Success = success;
    }
  }
}
