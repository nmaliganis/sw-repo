namespace sw.interprocess.api.Commanding.Events.EventArgs.Inbound
{
  public class TelemetryRowDetectionEventArgs : System.EventArgs
  {
    public string Payload { get; private set; }
    public bool Success { get; private set; }
    public string Imei { get; private set; }

    public TelemetryRowDetectionEventArgs(string payload, bool success, string imei)
    {
      Payload = payload;
      Success = success;
      Imei = imei;
    }
  }
}