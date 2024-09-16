using System;
using sw.auth.common.infrastructure.Commanding.Events;
using sw.auth.common.infrastructure.Commanding.Listeners;

namespace sw.auth.common.infrastructure.Commanding.Base
{
    /// <summary>
    /// Abstract class for Message BaseCommander
    /// </summary>
    public abstract class BaseCommander
    {
        #region Events - (public)
        /// <summary>
        /// Event for DriverCreationNotifier
        /// </summary>
        public event EventHandler<DriverCreationEventArgs> DriverCreationNotifier;

        #endregion

        #region Methods - Notify for Driver Creation

        private void OnDriverCreationNotification(DriverCreationEventArgs e)
        {
            DriverCreationNotifier?.Invoke(this, e);
        }

        /// <summary>
        /// Method for raising Async Event Notification
        /// </summary>

        public void RaiseDriverCreationNotification(string firstname, string lastname, string gender)
        {
            OnDriverCreationNotification(new DriverCreationEventArgs(firstname, lastname, gender));
        }

        /// <summary>
        /// Method to Subscribe listen Event for Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Attach(IDriverCreationNotificationActionListener listener)
        {
            DriverCreationNotifier += listener.Update;
        }

        /// <summary>
        /// Method to Unsubscribe listen Event Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Detach(IDriverCreationNotificationActionListener listener)
        {
            if (DriverCreationNotifier != null)
            {
                DriverCreationNotifier -= listener.Update;
            }
        }

        #endregion

    } // Class: BaseCommander
}