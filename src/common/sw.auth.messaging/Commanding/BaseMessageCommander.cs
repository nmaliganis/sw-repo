using System;
using sw.auth.messaging.Commanding.Events;
using sw.auth.messaging.Commanding.Listeners;

namespace sw.auth.messaging.Commanding
{

    /// <summary>
    /// Abstract class for Message Commander
    /// </summary>
    public abstract class BaseMessageCommander
    {

        #region Events - (public)


        /// <summary>
        /// Event for MemberRegistrationNotifier
        /// </summary>
        public event EventHandler<MemberRegistrationEventArgs> MemberRegistrationNotifier;


        #endregion


        #region Methods - Notify MemberRegistration

        private void OnModalNotification(MemberRegistrationEventArgs e)
        {
            MemberRegistrationNotifier?.Invoke(this, e);
        }

        /// <summary>
        /// Method for raising Async Event Modal Notification
        /// </summary>
        /// <param name="isSignOut"> SignOut indicator</param>
        public void RaiseModalNotification(bool isSignOut)
        {
            OnModalNotification(new MemberRegistrationEventArgs(isSignOut));
        }

        /// <summary>
        /// Method to Subscribe listen Event for Modal Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Attach(IMemberRegistrationNotificationActionListener listener)
        {
            MemberRegistrationNotifier += listener.Update;
        }

        /// <summary>
        /// Method to Unsubscribe listen Event for Modal Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Detach(IMemberRegistrationNotificationActionListener listener)
        {
            if (MemberRegistrationNotifier != null)
            {
                MemberRegistrationNotifier -= listener.Update;
            }
        }

        #endregion

    } // Class: BaseMessageCommander

}// Namespace : sw.auth.messaging.Commanding