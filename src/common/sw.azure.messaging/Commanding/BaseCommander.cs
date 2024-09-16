using sw.azure.messaging.Commanding.Events;
using sw.azure.messaging.Commanding.Listeners;
using sw.azure.messaging.Models.IoT;
using System;
using System.Collections.Generic;

namespace sw.azure.messaging.Commanding
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