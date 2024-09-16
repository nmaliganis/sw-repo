using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace sw.interprocess.api.Commanding.Listeners.Inbounds
{
    public interface IDigitalDetectionActionListener
    {
        void Update(object sender, DigitalDetectionEventArgs e);
    }
}