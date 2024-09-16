using sw.azure.messaging.Models.IoT;
using System;

namespace sw.azure.messaging.Commanding.Events
{

    /// <summary>
    /// Class EventArgs for IoT Message Notification Async 
    /// </summary>
    public class IoTGpsMessageEventArgs : System.EventArgs
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
        /// Property Timestamp
        /// </summary>
        public IoTgps PayloadGps { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Ctor for Member Registration Notification EventArgs
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <param name="imei"></param>
        /// <param name="timestamp"></param>
        /// <param name="payloadGps"></param>
        public IoTGpsMessageEventArgs(bool isEnabled, string imei, DateTime timestamp, IoTgps payloadGps)
        {
            IsEnabled = isEnabled;
            Imei = imei;
            Timestamp = timestamp;
            PayloadGps = payloadGps;
        }

        #endregion

    }//Class: IoTMessageEventArgs
}