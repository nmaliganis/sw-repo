using sw.auth.common.infrastructure.Commanding.Events;

namespace sw.auth.common.infrastructure.Commanding.Listeners
{
    /// <summary>
    /// Interface DriverCreationNotificationActionListener
    /// </summary>
    public interface IDriverCreationNotificationActionListener
    {
        /// <summary>
        /// Method Handler for Notifier
        /// </summary>
        /// <param name="sender"></param> Sender of the Command
        /// <param name="e"></param> Notifier Commander Event Args
        void Update(object sender, DriverCreationEventArgs e);

    } //Interface: DriverCreationNotificationActionListener
}