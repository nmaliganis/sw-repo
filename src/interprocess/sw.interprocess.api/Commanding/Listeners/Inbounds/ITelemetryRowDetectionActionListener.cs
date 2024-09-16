using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace sw.interprocess.api.Commanding.Listeners.Inbounds
{
  public interface ITelemetryRowDetectionActionListener
  {
    void Update(object sender, TelemetryRowDetectionEventArgs e);
  }
}