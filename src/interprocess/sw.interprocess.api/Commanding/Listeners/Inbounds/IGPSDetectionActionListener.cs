using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace sw.interprocess.api.Commanding.Listeners.Inbounds
{
    public interface IGPSDetectionActionListener
    {
        void Update(object sender, GPSDetectionEventArgs e);
    }
}