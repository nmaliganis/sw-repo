using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace sw.interprocess.api.Commanding.Listeners.Inbounds
{
  public interface IWsBroadcastMessageDetectionActionListener
  {
    void Update(object sender, WsBroadcastMessageDetectionEventArgs e);
  }
}