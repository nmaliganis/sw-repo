using sw.interprocess.api.Commanding.Events.EventArgs.Inbound;

namespace sw.interprocess.api.Commanding.Listeners.Inbounds
{
    public interface IAttributeDetectionActionListener
    {
        void Update(object sender, AttributeDetectionEventArgs e);
    }
}