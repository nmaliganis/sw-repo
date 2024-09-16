using sw.azure.messaging.Commanding.Events;
using sw.azure.messaging.Commanding.Listeners;
using sw.azure.messaging.Models.IoT;
using System;
using System.Collections.Generic;

namespace sw.azure.messaging.Commanding
{
    public abstract class BaseErrorCommander
    {
        #region Events - (public)
        /// <summary>
        /// Event for DriverCreationNotifier
        /// </summary>
        public event EventHandler<ErrorLoggingEventArgs> ErrorNotifier;

        #endregion

        #region Methods - Notify for Error Logging

        private void OnErrorLoggingNotification(ErrorLoggingEventArgs e)
        {
            ErrorNotifier?.Invoke(this, e);
        }

        /// <summary>
        /// Method for raising Async Event Notification
        /// </summary>

        public void RaiseErrorLoggingNotification()
        {
            OnErrorLoggingNotification(new ErrorLoggingEventArgs());
        }

        /// <summary>
        /// Method to Subscribe listen Event for Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Attach(IErrorLoggingNotificationActionListener listener)
        {
            ErrorNotifier += listener.Update;
        }

        /// <summary>
        /// Method to Unsubscribe listen Event Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Detach(IErrorLoggingNotificationActionListener listener)
        {
            if (ErrorNotifier != null)
            {
                ErrorNotifier -= listener.Update;
            }
        }

        #endregion

    } // Class: BaseErrorCommander
}