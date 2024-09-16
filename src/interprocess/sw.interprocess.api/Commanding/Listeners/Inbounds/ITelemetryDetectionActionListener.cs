using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace sw.interprocess.api.Commanding.Listeners.Inbounds
{
    public interface ITelemetryDetectionActionListener
    {
        void Update(object sender, TelemetryDetectionEventArgs e);
    }
}