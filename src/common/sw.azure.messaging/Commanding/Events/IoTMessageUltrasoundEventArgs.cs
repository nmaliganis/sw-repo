using sw.azure.messaging.Models.IoT;
using System;
using System.Collections.Generic;

namespace sw.azure.messaging.Commanding.Events
{

    /// <summary>
    /// Class EventArgs for IoT Message Notification Async 
    /// </summary>
    public class IoTUltrasonicMessageEventArgs : System.EventArgs
    {
        #region Properties - Variables (public)

        /// <summary>
        /// Property IsEnabled
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Property Imei
        /// </summary>
        public string Imei { get; private set; }

        /// <summary>
        /// Property Timestamp
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Property PayloadUltrasonic
        /// </summary>
        public List<IoTUltrasonic> PayloadUltrasonic { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Ctor for Member Registration Notification EventArgs
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <param name="imei"></param>
        /// <param name="timestamp"></param>
        /// <param name="payloadUltrasonic"></param>
        public IoTUltrasonicMessageEventArgs(bool isEnabled, string imei, DateTime timestamp, List<IoTUltrasonic> payloadUltrasonic)
        {
            IsEnabled = isEnabled;
            Imei = imei;
            Timestamp = timestamp;
            PayloadUltrasonic = payloadUltrasonic;
        }

        #endregion

    }//Class: IoTUltrasoundMessageEventArgs
}