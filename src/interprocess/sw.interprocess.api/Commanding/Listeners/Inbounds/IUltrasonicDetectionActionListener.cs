using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace sw.interprocess.api.Commanding.Listeners.Inbounds
{
  public interface IUltrasonicDetectionActionListener
  {
    void Update(object sender, UltrasonicDetectionEventArgs e);
  }
}