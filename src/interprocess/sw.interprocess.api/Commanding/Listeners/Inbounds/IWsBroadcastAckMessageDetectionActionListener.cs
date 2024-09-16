using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace sw.interprocess.api.Commanding.Listeners.Inbounds
{
  public interface IWsBroadcastAckMessageDetectionActionListener
  {
    void Update(object sender, WsBroadcastAckMessageDetectionEventArgs e);
  }
}