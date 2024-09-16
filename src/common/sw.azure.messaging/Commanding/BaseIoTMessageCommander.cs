using sw.azure.messaging.Commanding.Events;
using sw.azure.messaging.Commanding.Listeners;
using sw.azure.messaging.Models.IoT;
using System;
using System.Collections.Generic;

namespace sw.azure.messaging.Commanding
{
    /// <summary>
    /// Abstract class for Message Commander
    /// </summary>
    public abstract class BaseIoTMessageCommander
    {
        #region Events - (public)
        /// <summary>
        /// Event for IoTUltrasoundNotifier
        /// </summary>
        public event EventHandler<IoTUltrasonicMessageEventArgs> IoTUltrasonicNotifier;

        /// <summary>
        /// Event for IoTUltrasoundNotifier
        /// </summary>
        public event EventHandler<IoTGpsMessageEventArgs> IoTGpsNotifier;

        /// <summary>
        /// Event for IoTDigitalNotifier
        /// </summary>
        public event EventHandler<IoTDigitalMessageEventArgs> IoTDigitalNotifier;

        #endregion

        #region Methods - Notify Ultrasound Notification

        private void OnUltrasonicNotification(IoTUltrasonicMessageEventArgs e)
        {
            IoTUltrasonicNotifier?.Invoke(this, e);
        }

        /// <summary>
        /// Method for raising Async Event Notification
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="imei"></param>
        /// <param name="timestamp"></param>
        /// <param name="payloadUltrasonic"></param>
        public void RaiseUltrasonicNotification(bool enabled, string imei, DateTime timestamp, List<IoTUltrasonic> payloadUltrasonic)
        {
            OnUltrasonicNotification(new IoTUltrasonicMessageEventArgs(enabled, imei, timestamp, payloadUltrasonic));
        }

        /// <summary>
        /// Method to Subscribe listen Event for Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Attach(IIoTUltrasonicNotificationActionListener listener)
        {
            IoTUltrasonicNotifier += listener.Update;
        }

        /// <summary>
        /// Method to Unsubscribe listen Event Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Detach(IIoTUltrasonicNotificationActionListener listener)
        {
            if (IoTUltrasonicNotifier != null)
            {
                IoTUltrasonicNotifier -= listener.Update;
            }
        }

        #endregion

        #region Methods - Notify Gps Notification

        private void OnGpsNotification(IoTGpsMessageEventArgs e)
        {
            IoTGpsNotifier?.Invoke(this, e);
        }

        /// <summary>
        /// Method for raising Async Event Notification
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="imei"></param>
        /// <param name="timestamp"></param>
        /// <param name="payloadGps"></param>
        public void RaiseGpsNotification(bool enabled, string imei, DateTime timestamp, IoTgps payloadGps)
        {
            OnGpsNotification(new IoTGpsMessageEventArgs(enabled, imei, timestamp, payloadGps));
        }

        /// <summary>
        /// Method to Subscribe listen Event for Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Attach(IIoTGpsNotificationActionListener listener)
        {
            IoTGpsNotifier += listener.Update;
        }

        /// <summary>
        /// Method to Unsubscribe listen Event Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Detach(IIoTGpsNotificationActionListener listener)
        {
            if (IoTGpsNotifier != null)
            {
                IoTGpsNotifier -= listener.Update;
            }
        }

        #endregion

        #region Methods - Notify Digital Notification

        private void OnDigitalNotification(IoTDigitalMessageEventArgs e)
        {
            IoTDigitalNotifier?.Invoke(this, e);
        }

        /// <summary>
        /// Method for raising Async Event Notification
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="imei"></param>
        /// <param name="timestamp"></param>
        /// <param name="payloadGps"></param>
        public void RaiseDigitalNotification(bool enabled, string imei, DateTime timestamp, List<IoTDigitalEvent> payloadDigital)
        {
            OnDigitalNotification(new IoTDigitalMessageEventArgs(enabled, imei, timestamp, payloadDigital));
        }

        /// <summary>
        /// Method to Subscribe listen Event for Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Attach(IIoTDigitalNotificationActionListener listener)
        {
            IoTDigitalNotifier += listener.Update;
        }

        /// <summary>
        /// Method to Unsubscribe listen Event Notification
        /// </summary>
        /// <param name="listener"></param>
        public void Detach(IIoTDigitalNotificationActionListener listener)
        {
            if (IoTDigitalNotifier != null)
            {
                IoTDigitalNotifier -= listener.Update;
            }
        }

        #endregion

    } // Class: BaseMessageCommander
}