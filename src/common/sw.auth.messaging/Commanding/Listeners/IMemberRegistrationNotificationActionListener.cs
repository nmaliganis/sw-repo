using sw.auth.messaging.Commanding.Events;

namespace sw.auth.messaging.Commanding.Listeners
{
    /// <summary>
    /// Interface for MemberRegistration Notification ActionListener
    /// </summary>
    public interface IMemberRegistrationNotificationActionListener
    {
        /// <summary>
        /// Method Handler for Notifier
        /// </summary>
        /// <param name="sender"></param> Sender of the Command
        /// <param name="e"></param> Notifier Commander Event Args
        void Update(object sender, MemberRegistrationEventArgs e);

    }//Interface: IMemberRegistrationNotificationActionListener

}// Namespace : sw.auth.messaging.Commanding.Listeners