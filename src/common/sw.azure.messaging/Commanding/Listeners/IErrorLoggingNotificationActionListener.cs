using sw.azure.messaging.Commanding.Events;

namespace sw.azure.messaging.Commanding.Listeners
{
    public interface IErrorLoggingNotificationActionListener
    {
        void Update(object sender, ErrorLoggingEventArgs e);

    } 
}